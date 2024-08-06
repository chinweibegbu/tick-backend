using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TemplateTest.Domain.Common
{
    public static class CoreHelpers
    {
        private static readonly long _baseDateTicks = new DateTime(1900, 1, 1).Ticks;
        private static readonly DateTime _epoc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly Random _random = new Random();

        public static Ulid CreateUlid(DateTimeOffset timestamp)
        {
            string randomness = RandomString(16, lower: false);
            Span<byte> randomnessBytes = stackalloc byte[10];
            randomnessBytes = GenerateRandomBytes(10);
            return Ulid.NewUlid(timestamp, randomnessBytes);
        }

        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        public static long ToEpocMilliseconds(DateTime date)
        {
            return (long)Math.Round((date - _epoc).TotalMilliseconds, 0);
        }

        public static DateTime FromEpocMilliseconds(long milliseconds)
        {
            return _epoc.AddMilliseconds(milliseconds);
        }

        public static long ToEpocSeconds(DateTime date)
        {
            return (long)Math.Round((date - _epoc).TotalSeconds, 0);
        }

        public static DateTime FromEpocSeconds(long seconds)
        {
            return _epoc.AddSeconds(seconds);
        }

        public static string RandomString(int length, bool alpha = true, bool upper = true, bool lower = true,
            bool numeric = true, bool special = false)
        {
            return RandomString(length, RandomStringCharacters(alpha, upper, lower, numeric, special));
        }

        public static IList<T> ReadCsvStream<T>(Stream stream, bool skipFirstLine = true, string csvDelimiter = ",") where T : new()
        {
            var records = new List<T>();
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(csvDelimiter.ToCharArray());
                    if (skipFirstLine)
                    {
                        skipFirstLine = false;
                    }
                    else
                    {
                        var item = new T();
                        var properties = item.GetType().GetProperties();
                        for (int i = 0; i < values.Length; i++)
                        {
                            properties[i].SetValue(item, Convert.ChangeType(values[i], properties[i].PropertyType, CultureInfo.CurrentCulture), null);
                        }

                        records.Add(item);
                    }
                }
            }

            return records;
        }

        public static byte[] ExportCsv<T>(IList<T> data, bool includeHeader = true, string csvDelimiter = ",")
        {
            var type = data.GetType();
            Type itemType;

            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                itemType = type.GetElementType();
            }

            using (var stringWriter = new StringWriter())
            {
                if (includeHeader)
                {
                    stringWriter.WriteLine(
                        string.Join<string>(
                            csvDelimiter, itemType.GetProperties().Select(x => x.Name)
                        )
                    );
                }

                foreach (var obj in data)
                {
                    var vals = obj.GetType().GetProperties().Select(pi => new
                    {
                        Value = pi.GetValue(obj, null)
                    }
                    );

                    string line = string.Empty;
                    foreach (var val in vals)
                    {
                        if (val.Value != null)
                        {
                            var escapeVal = val.Value.ToString();
                            // Check if the value contans a comma and place it in quotes if so
                            if (escapeVal.Contains(',', StringComparison.OrdinalIgnoreCase))
                            {
                                escapeVal = string.Concat("\"", escapeVal, "\"");
                            }

                            // Replace any \r or \n special characters from a new line with a space
                            if (escapeVal.Contains('\r', StringComparison.OrdinalIgnoreCase))
                            {
                                escapeVal = escapeVal.Replace("\r", " ", StringComparison.OrdinalIgnoreCase);
                            }

                            if (escapeVal.Contains('\n', StringComparison.OrdinalIgnoreCase))
                            {
                                escapeVal = escapeVal.Replace("\n", " ", StringComparison.OrdinalIgnoreCase);
                            }

                            line = string.Concat(line, escapeVal, csvDelimiter);
                        }
                        else
                        {
                            line = string.Concat(line, string.Empty, csvDelimiter);
                        }
                    }

                    stringWriter.WriteLine(line.TrimEnd(csvDelimiter.ToCharArray()));
                }

                var csvBytes = Encoding.UTF8.GetBytes(stringWriter.ToString());
                // MS Excel need the BOM to display UTF8 Correctly
                return Encoding.UTF8.GetPreamble().Concat(csvBytes).ToArray();
            }
        }

        public static string RandomString(int length, string characters)
        {
            return new string(Enumerable.Repeat(characters, length).Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public static string SecureRandomString(int length, bool alpha = true, bool upper = true, bool lower = true,
            bool numeric = true, bool special = false)
        {
            return SecureRandomString(length, RandomStringCharacters(alpha, upper, lower, numeric, special));
        }

        // ref https://stackoverflow.com/a/8996788/1090359 with modifications
        public static string SecureRandomString(int length, string characters)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
            }

            if ((characters?.Length ?? 0) == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(characters), "characters invalid.");
            }

            const int byteSize = 0x100;
            if (byteSize < characters.Length)
            {
                throw new ArgumentException(
                    string.Format("{0} may contain no more than {1} characters.", nameof(characters), byteSize),
                    nameof(characters));
            }

            var outOfRangeStart = byteSize - (byteSize % characters.Length);
            using (var rng = RandomNumberGenerator.Create())
            {
                var sb = new StringBuilder();
                var buffer = new byte[128];
                while (sb.Length < length)
                {
                    rng.GetBytes(buffer);
                    for (var i = 0; i < buffer.Length && sb.Length < length; ++i)
                    {
                        // Divide the byte into charSet-sized groups. If the random value falls into the last group and the
                        // last group is too small to choose from the entire allowedCharSet, ignore the value in order to
                        // avoid biasing the result.
                        if (outOfRangeStart <= buffer[i])
                        {
                            continue;
                        }

                        sb.Append(characters[buffer[i] % characters.Length]);
                    }
                }

                return sb.ToString();
            }
        }

        private static string RandomStringCharacters(bool alpha, bool upper, bool lower, bool numeric, bool special)
        {
            var characters = string.Empty;
            if (alpha)
            {
                if (upper)
                {
                    characters += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                }

                if (lower)
                {
                    characters += "abcdefghijklmnopqrstuvwxyz";
                }
            }

            if (numeric)
            {
                characters += "0123456789";
            }

            if (special)
            {
                characters += "!@#$%^*&";
            }

            return characters;
        }

        // ref: https://stackoverflow.com/a/11124118/1090359
        // Returns the human-readable file size for an arbitrary 64-bit file size .
        // The format is "0.## XB", ex: "4.2 KB" or "1.43 GB"
        public static string ReadableBytesSize(long size)
        {
            // Get absolute value
            var absoluteSize = (size < 0 ? -size : size);

            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absoluteSize >= 0x40000000) // 1 Gigabyte
            {
                suffix = "GB";
                readable = (size >> 20);
            }
            else if (absoluteSize >= 0x100000) // 1 Megabyte
            {
                suffix = "MB";
                readable = (size >> 10);
            }
            else if (absoluteSize >= 0x400) // 1 Kilobyte
            {
                suffix = "KB";
                readable = size;
            }
            else
            {
                return size.ToString("0 Bytes"); // Byte
            }

            // Divide by 1024 to get fractional value
            readable /= 1024;

            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
        }

        /// <summary>
        /// Creates a clone of the given object through serializing to json and deserializing.
        /// This method is subject to the limitations of System.Text.Json. For example, properties with
        /// inaccessible setters will not be set.
        /// </summary>
        public static T CloneObject<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        public static string Base64EncodeString(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        public static string Base64DecodeString(string input)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(input));
        }

        public static string Base64UrlEncodeString(string input)
        {
            return Base64UrlEncode(Encoding.UTF8.GetBytes(input));
        }

        public static string Base64UrlDecodeString(string input)
        {
            return Encoding.UTF8.GetString(Base64UrlDecode(input));
        }

        public static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", string.Empty);
            return output;
        }

        public static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            // 62nd char of encoding
            output = output.Replace('-', '+');
            // 63rd char of encoding
            output = output.Replace('_', '/');
            // Pad with trailing '='s
            switch (output.Length % 4)
            {
                case 0:
                    // No pad chars in this case
                    break;
                case 2:
                    // Two pad chars
                    output += "=="; break;
                case 3:
                    // One pad char
                    output += "="; break;
                default:
                    throw new InvalidOperationException("Illegal base64url string!");
            }

            // Standard base64 decoder
            return Convert.FromBase64String(output);
        }

        public static string SanitizeForEmail(string value, bool htmlEncode = true)
        {
            var cleanedValue = value.Replace("@", "[at]");
            var regexOptions = RegexOptions.CultureInvariant |
                RegexOptions.Singleline |
                RegexOptions.IgnoreCase;
            cleanedValue = Regex.Replace(cleanedValue, @"(\.\w)",
                    m => string.Concat("[dot]", m.ToString().Last()), regexOptions);
            while (Regex.IsMatch(cleanedValue, @"((^|\b)(\w*)://)", regexOptions))
            {
                cleanedValue = Regex.Replace(cleanedValue, @"((^|\b)(\w*)://)",
                    string.Empty, regexOptions);
            }
            return htmlEncode ? HttpUtility.HtmlEncode(cleanedValue) : cleanedValue;
        }

        // ref: https://stackoverflow.com/a/27545010/1090359
        public static Uri ExtendQuery(Uri uri, IDictionary<string, string> values)
        {
            var baseUri = uri.ToString();
            var queryString = string.Empty;
            if (baseUri.Contains('?'))
            {
                var urlSplit = baseUri.Split('?');
                baseUri = urlSplit[0];
                queryString = urlSplit.Length > 1 ? urlSplit[1] : string.Empty;
            }

            var queryCollection = HttpUtility.ParseQueryString(queryString);
            foreach (var kvp in values ?? new Dictionary<string, string>())
            {
                queryCollection[kvp.Key] = kvp.Value;
            }

            var uriKind = uri.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative;
            if (queryCollection.Count == 0)
            {
                return new Uri(baseUri, uriKind);
            }
            return new Uri(string.Format("{0}?{1}", baseUri, queryCollection), uriKind);
        }

        public static string Aes128CbcEncryption<T>(T value, string key, string iv)
        {
            var json = JsonConvert.SerializeObject(value);

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);

            var encrypted = AesEncrypt(json, keyBytes, ivBytes);

            return Convert.ToHexString(encrypted);
        }

        public static T Aes128CbcDecryption<T>(string value, string key, string iv)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var ivBytes = Encoding.UTF8.GetBytes(iv);

            var encrypted = Convert.FromHexString(value);

            var decrypted = AesDecrypt(encrypted, keyBytes, ivBytes);

            return JsonConvert.DeserializeObject<T>(decrypted);
        }

        static byte[] AesEncrypt(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("Encryption text");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Encryption Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Encryption IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        static string AesDecrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static T LoadClassFromJsonData<T>(string jsonData) where T : new()
        {
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return new T();
            }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public static string ClassToJsonData<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static ICollection<T> AddIfNotExists<T>(this ICollection<T> list, T item)
        {
            if (list.Contains(item))
            {
                return list;
            }
            list.Add(item);
            return list;
        }

        public static string ObfuscateEmail(string email)
        {
            if (email == null)
            {
                return email;
            }

            var emailParts = email.Split('@', StringSplitOptions.RemoveEmptyEntries);

            if (emailParts.Length != 2)
            {
                return email;
            }

            var username = emailParts[0];

            if (username.Length < 2)
            {
                return email;
            }

            var sb = new StringBuilder();
            sb.Append(emailParts[0][..2]);
            for (var i = 2; i < emailParts[0].Length; i++)
            {
                sb.Append('*');
            }

            return sb.Append('@')
                .Append(emailParts[1])
                .ToString();

        }

        public static string GetEmailDomain(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var emailParts = email.Split('@', StringSplitOptions.RemoveEmptyEntries);

                if (emailParts.Length == 2)
                {
                    return emailParts[1].Trim();
                }
            }

            return null;
        }
    }
}
