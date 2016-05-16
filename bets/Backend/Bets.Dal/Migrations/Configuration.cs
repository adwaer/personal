using Bets.Domain;

namespace Bets.Dal.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bets.Dal.DefaultCtx>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Bets.Dal.DefaultCtx context)
        {
            if (context.Bets.Any())
            {
                return;
            }

            context.Bets.Add(new Bet
            {
                Game = "Гласспул Ллойд - Уиттингтон Эндрю",
                Tournament = "Теннис. ATP Челленджер",
                Content = "Ставка верняк, Эндрю победит",
                Coefficient = (decimal)1.7,
                Forecast = "П2",
                GameStartDate = DateTime.UtcNow.AddHours(10),
                MakeDate = DateTime.UtcNow,
                ShowDate = DateTime.UtcNow
            });

            context.Bets.Add(new Bet
            {
                Game = "Россия - Англия",
                Tournament = "Футбол. Чемпионат Европпы 2016",
                Content = "Россия форме и забитых голов будет не менее двух",
                Coefficient = (decimal)2.3,
                Forecast = "ТМ2,5",
                GameStartDate = DateTime.UtcNow.AddHours(7),
                MakeDate = DateTime.UtcNow,
                ShowDate = DateTime.UtcNow
            });
        }
    }
}
