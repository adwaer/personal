using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Personal.Domain.Entities;

namespace Personal.User
{
    public class MyUserManager : UserManager<Customer, int>
    {
        public MyUserManager(IUserStore<Customer, int> store) : base(store)
        {
            UserTokenProvider = new TotpSecurityStampBasedTokenProvider<Customer, int>();
            UserValidator = new UserValidator<Customer, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public static MyUserManager Get(DbContext dncontext)
        {
            return new MyUserManager(new MyUserStore(dncontext));
        }
        
    }
}
