using System.IO;

namespace MQ.Domain
{
    /// <summary>
    /// Запись с геоинформацией
    /// </summary>
    public class IpLocation
    {
        /// <summary>
        /// начало диапозона IP адресов
        /// </summary>
        public ulong FromIp { get; set; }
        /// <summary>
        /// конец диапозона IP адресов
        /// </summary>
        public ulong ToIp { get; set; }
        /// <summary>
        /// индекс записи о местоположении
        /// </summary>
        public uint Index { get; set; }

        public static IpLocation Get(BinaryReader reader)
        {
            return new IpLocation
            {
                FromIp = reader.ReadUInt64(),
                ToIp = reader.ReadUInt64(),
                Index = reader.ReadUInt32()
            };
        }

        public override string ToString()
        {
            return $"{FromIp} {ToIp} {Index}";
        }
    }
}
