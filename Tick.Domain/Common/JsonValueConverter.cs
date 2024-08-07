using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Tick.Domain.Common
{
    public class JsonValueConverter<T> : ValueConverter<T, string> where T : class
    {
        public JsonValueConverter(ConverterMappingHints hints = default)
            : base(v => Serialize(v), v => Deserialize(v), hints)
        {
        }

        static T Deserialize(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        static string Serialize(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
