﻿using CostEffectiveCode.Domain.Ddd.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personal.Domain.Entities
{
    public class Customer : IdentityUser<int, IdentityUserLogin<int>, UserRole, Claim>, IEntityBase<int>
    {
        public string GetId()
        {
            return Id.ToString();
        }
    }
}
