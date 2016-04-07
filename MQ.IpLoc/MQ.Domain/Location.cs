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

        public void Fill(string country, string region, string postal, string city, string company, float lat, float lon)
        {
            Country = country;
            Region = region;
            Postal = postal;
            City = city;
            Company = company;
            Lat = lat;
            Lon = lon;
        }

        public override string ToString()
        {
            return $"{Country} {Region} {Postal} {City} {Company} {Lat}/{Lon}";
        }
    }
}
