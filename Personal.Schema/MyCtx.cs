using System.Data.Entity;
using CostEffectiveCode.EntityFramework6;
using Personal.Domain.Entities;

namespace Personal.Schema
{
    public class MyCtx : DbContextBase
    {
        public MyCtx()
            : base("ctx")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
