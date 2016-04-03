using System;
using CostEffectiveCode.Domain.Ddd.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personal.Domain.Entities
{
    public class UserRole : IdentityUserRole<int>, IEntityBase<int>
    {
        public string GetId()
        {
            return Id.ToString();
        }

        public int Id { get; set; }
        public virtual  Customer User { get; set; }
    }
}
