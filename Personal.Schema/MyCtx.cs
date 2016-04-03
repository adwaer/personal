using System;
using System.Data.Entity;
using System.Diagnostics;
using CostEffectiveCode.EntityFramework6;
using Microsoft.AspNet.Identity.EntityFramework;
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
#if DEBUG
            Database.Log = s => Debug.WriteLine(s);
#endif
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Claim> IdentityUserClaims { get; set; }

        public static MyCtx Create()
        {
            return new MyCtx();
        }
    }
}
