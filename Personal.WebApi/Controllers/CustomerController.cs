using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Personal.Domain.Entities;

namespace Personal.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly UserManager<Customer, int> _userManager;

        public CustomerController(UserManager<Customer, int> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IHttpActionResult> Get()
        {
            string login = string.Empty, password = string.Empty;

            IEnumerable<KeyValuePair<string, string>> pars = Request.GetQueryNameValuePairs();
            foreach (var pair in pars)
            {
                if (pair.Key.Equals("login", StringComparison.CurrentCultureIgnoreCase))
                {
                    login = pair.Value;
                }
                if (pair.Key.Equals("pass", StringComparison.CurrentCultureIgnoreCase))
                {
                    password = pair.Value;
                }
            }

            var customer = await _userManager.FindAsync(login, password);
            if (customer != null)
            {
                var identity = await _userManager.CreateIdentityAsync(customer, DefaultAuthenticationTypes.ApplicationCookie);
                Thread.CurrentPrincipal = new ClaimsPrincipal(identity);
                return Ok();
            }

            return BadRequest("login_fail");
        }
    }
}
