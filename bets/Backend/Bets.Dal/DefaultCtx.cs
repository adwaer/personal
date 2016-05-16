using System.Data.Entity;
using Bets.Domain;

namespace Bets.Dal
{
    public class DefaultCtx : DbContext
    {
        public DefaultCtx()
            :base("ctx")
        {
        }

        public DbSet<Bet> Bets { get; set; }
    }
}
