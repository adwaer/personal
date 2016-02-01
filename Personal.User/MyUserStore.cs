using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Personal.Domain.Entities;

namespace Personal.User
{
    public class MyUserStore : UserStore<Customer, IdentityRole<int, IdentityUserRole<int>>, int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>>
    {
        public MyUserStore(DbContext ctx) : base(ctx) { }

        /// <summary>
        /// Insert an entity
        /// </summary>
        /// <param name="user"/>
        public override Task CreateAsync(Customer user)
        {
            Context.Set<Customer>().Add(user);

            Context.Entry(user).State = EntityState.Added;
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="user"/>
        public override Task UpdateAsync(Customer user)
        {
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Mark an entity for deletion
        /// </summary>
        /// <param name="user"/>
        public override Task DeleteAsync(Customer user)
        {
            return UpdateAsync(user);
        }
        /// <summary>
        /// Find a user by id
        /// </summary>
        /// <param name="userId"/>
        /// <returns/>
        public override async Task<Customer> FindByIdAsync(int userId)
        {
            return await Context.Set<Customer>().FirstOrDefaultAsync(u => u.Id == userId);
        }
        /// <summary>
        /// Find a user by name
        /// </summary>
        /// <param name="userName"/>
        /// <returns/>
        public override async Task<Customer> FindByNameAsync(string userName)
        {
            return await Context.Set<Customer>().FirstOrDefaultAsync(u => Equals(u.UserName, userName));
        }
    }
}
