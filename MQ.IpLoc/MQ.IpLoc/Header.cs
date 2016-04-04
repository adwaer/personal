﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.IpLoc
{
    public class Header
    {
        /// <summary>
        /// версия база данных
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// название/префикс для базы данных
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// время создания базы данных
        /// </summary>
        public DateTime MakeTime { get; set; }
        /// <summary>
        /// общее количество записей
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// смещение относительно начала файла до начала списка записей с геоинформацией
        /// </summary>
        public uint RangeOffset { get; set; }
        /// <summary>
        /// смещение относительно начала файла до начала индекса с сортировкой по названию городов
        /// </summary>
        public uint CityOffset { get; set; }
        /// <summary>
        /// смещение относительно начала файла до начала списка записей о местоположении
        /// </summary>
        public uint LocationOffset { get; set; }

        public static Header Get(BinaryReader reader)
        {
            return new Header
            {
                Version = reader.ReadInt32(),
                Name = Encoding.Default
                    .GetString(reader.ReadBytes(32))
                    .TrimEnd(EnvironmentConstant.EmptySpace),
                MakeTime = EnvironmentConstant.UnixDateTime.AddSeconds(reader.ReadInt64()).ToLocalTime()
            };
        }
    }
}
