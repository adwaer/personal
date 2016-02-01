using System;
using CostEffectiveCode.Domain.Ddd.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personal.Domain.Entities
{
    public class Customer : IdentityUser<int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>>, IEntityBase<int>
    {
        public string GetId()
        {
            return Id.ToString();
        }
    }
}
