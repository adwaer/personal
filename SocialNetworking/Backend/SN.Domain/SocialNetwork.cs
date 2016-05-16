using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SN.Domain
{
    public class SocialNetwork :IEntity<int>
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
    }
}
