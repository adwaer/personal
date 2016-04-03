using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Personal.User;

namespace Personal.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly IAuthManager _authManager;
        public CustomerController(IAuthManager authManager)
        {
            _authManager = authManager;
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

            if (await _authManager.ValidateAsync(login, password))
            {
                return Ok();
            }

            return BadRequest("login_fail");
        }
    }
}
