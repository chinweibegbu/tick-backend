using Tick.Core.Contract;
using System.IO;
using System.Runtime.Serialization;

namespace Tick.Core.Serializers
{
    public class BinarySerializer : IBinarySerializer
    {
        /// <summary>  </summary>
        public byte[] Serialize<T>(T value) where T : notnull
        {
            using var ms = new MemoryStream();
            var serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(ms, value);
            return ms.ToArray();
        }

#nullable enable
        /// <summary>  </summary>
        public T? Deserialize<T>(byte[] value) where T : notnull
        {
            using var memStream = new MemoryStream(value);
            var serializer = new DataContractSerializer(typeof(T));
            var obj = (T?)serializer.ReadObject(memStream);
            return obj;
        }
#nullable disable
    }
}
