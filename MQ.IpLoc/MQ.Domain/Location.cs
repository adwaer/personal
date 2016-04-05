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
        public string Country { get; private set; }
        /// <summary>
        /// название области (случайная строка с префиксом "reg_")
        /// </summary>
        public string Region { get; private set; }
        /// <summary>
        /// почтовый индекс (случайная строка с префиксом "pos_")
        /// </summary>
        public string Postal { get; private set; }
        /// <summary>
        /// название города (случайная строка с префиксом "cit_")
        /// </summary>
        public string City { get; private set; }
        /// <summary>
        /// название организации (случайная строка с префиксом "org_")
        /// </summary>
        public string Company { get; private set; }
        /// <summary>
        /// широта
        /// </summary>
        public float Lat { get; private set; }
        /// <summary>
        /// долгота
        /// </summary>
        public float Lon { get; private set; }

        public static Location Get(BinaryReader reader)
        {
            return new Location
            {
                Country = EnvironmentConstant.GetStringFromSbyte(reader, 8),
                Region = EnvironmentConstant.GetStringFromSbyte(reader, 12),
                Postal = EnvironmentConstant.GetStringFromSbyte(reader, 12),
                City = EnvironmentConstant.GetStringFromSbyte(reader, 24),
                Company = EnvironmentConstant.GetStringFromSbyte(reader, 32),
                Lat = EnvironmentConstant.ReadFloat(reader),
                Lon = EnvironmentConstant.ReadFloat(reader)
            };
        }
    }
}
