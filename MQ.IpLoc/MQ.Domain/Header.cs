using System;
using System.IO;
using System.Text;

namespace MQ.Domain
{
    /// <summary>
    /// Заголовок файла данных
    /// </summary>
    public class Header
    {
        /// <summary>
        /// версия база данных
        /// </summary>
        public int Version { get; private set; }
        /// <summary>
        /// название/префикс для базы данных
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// время создания базы данных
        /// </summary>
        public DateTime MakeTime { get; private set; }
        /// <summary>
        /// общее количество записей
        /// </summary>
        public int RecordCount { get; private set; }
        /// <summary>
        /// смещение относительно начала файла до начала списка записей с геоинформацией
        /// </summary>
        public uint RangeOffset { get; private set; }
        /// <summary>
        /// смещение относительно начала файла до начала индекса с сортировкой по названию городов
        /// </summary>
        public uint CityOffset { get; private set; }
        /// <summary>
        /// смещение относительно начала файла до начала списка записей о местоположении
        /// </summary>
        public uint LocationOffset { get; private set; }

        public static Header Get(BinaryReader reader)
        {
            return new Header
            {
                Version = reader.ReadInt32(),
                Name = Encoding.Default
                    .GetString(reader.ReadBytes(32))
                    .TrimEnd(EnvironmentConstant.EmptySpace),
                MakeTime = EnvironmentConstant.UnixDateTime.AddSeconds(reader.ReadInt64()).ToLocalTime(),
                RecordCount = reader.ReadInt32(),
                RangeOffset = reader.ReadUInt32(),
                CityOffset = reader.ReadUInt32(),
                LocationOffset = reader.ReadUInt32()
            };
        }
    }
}
