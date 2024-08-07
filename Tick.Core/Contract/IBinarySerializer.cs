namespace Tick.Core.Contract
{
    public interface IBinarySerializer
    {
        /// <summary>  </summary>
        byte[] Serialize<T>(T value) where T : notnull;

#nullable enable
        /// <summary>  </summary>
        T? Deserialize<T>(byte[] value) where T : notnull;
#nullable disable
    }
}
