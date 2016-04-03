using CostEffectiveCode.Domain.Ddd.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Personal.Domain.Entities
{
    public class Claim: IdentityUserClaim<int>, IEntityBase<int>
    {
        public Customer User { get; set; }
        public string GetId()
        {
            return Id.ToString();
        }
    }
}
