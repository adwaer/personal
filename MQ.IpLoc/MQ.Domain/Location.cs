using System.IO;

namespace MQ.Domain
{
    /// <summary>
    /// cписок записей с информацией о местоположении с координатами (долгота и широта)
    /// </summary>
    public class Location
    {
        /// <summary>
        /// название страны (случайная строка с префиксом "cou_")
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// название области (случайная строка с префиксом "reg_")
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// почтовый индекс (случайная строка с префиксом "pos_")
        /// </summary>
        public string Postal { get; set; }
        /// <summary>
        /// название города (случайная строка с префиксом "cit_")
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// название организации (случайная строка с префиксом "org_")
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// широта
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// долгота
        /// </summary>
        public float Lon { get; set; }

        public static Location Get(BinaryReader reader)
        {
            return new Location
            {
                Country = EnvironmentConstant.ReadString(reader, 8),
                Region = EnvironmentConstant.ReadString(reader, 12),
                Postal = EnvironmentConstant.ReadString(reader, 12),
                City = EnvironmentConstant.ReadString(reader, 24),
                Company = EnvironmentConstant.ReadString(reader, 32),
                Lat = reader.ReadSingle(),
                Lon = reader.ReadSingle()
            };
        }

        public override string ToString()
        {
            return $"{Country} {Region} {Postal} {City} {Company} {Lat}/{Lon}";
        }
    }
}
