using System;

namespace Bets.Domain
{
    public class Bet: EntityBase<int>
    {
        public string Game { get; set; }
        public string Tournament { get; set; }
        public string Forecast { get; set; }
        public string Content { get; set; }
        public decimal Coefficient { get; set; }
        public DateTime GameStartDate { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime MakeDate { get; set; }
    }
}
