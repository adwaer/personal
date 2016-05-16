using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Bets.Domain;

namespace Bets.Cqrs.Query
{
    public class BetsQuery : IQuery<DateTime, DateTime, IQueryable<Bet>>
    {
        private readonly DbContext _dbContext;

        public BetsQuery(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Bet> Execute(DateTime startDate, DateTime endDate)
        {
            return _dbContext
                .Set<Bet>()
                .Where(bet => bet.ShowDate >= startDate && bet.ShowDate <= endDate);
        }
    }
}
