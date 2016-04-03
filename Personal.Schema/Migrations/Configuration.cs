using System.Configuration;
using System.Linq;
using Microsoft.AspNet.Identity;
using Personal.Domain.Entities;
using Personal.User;

namespace Personal.Schema.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MyCtx>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyCtx context)
        {
            if (!context.Customers.Any())
            {
                var result = MyUserManager.Get(context).Create(new Customer { UserName = "adwaer" }, "ghwefg43");
                if (!result.Succeeded)
                {
                    throw new ConfigurationErrorsException("Cannot set password for admin user");
                }
            }
        }
    }
}
