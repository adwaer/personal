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

        public override string ToString()
        {
            return $"{FromIp} {ToIp} {Index}";
        }
    }
}
