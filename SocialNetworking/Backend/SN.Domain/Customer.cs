using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SN.Domain
{
    class Customer : IEntity<int>
    {
        public int Id { get; set; }
        [Index("IX_Email", 1, IsUnique = true)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public ICollection<CustomerSocialNetwork> CustomerSocialNetworks { get; set; }
    }
}
