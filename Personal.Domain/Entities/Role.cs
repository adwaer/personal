using CostEffectiveCode.Domain.Ddd.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personal.Domain.Entities
{
    public class Role: IdentityRole<int, UserRole>, IEntityBase<int>
    {
        public string GetId()
        {
            return Id.ToString();
        }
    }
}
