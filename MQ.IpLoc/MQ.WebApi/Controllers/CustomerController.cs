using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MQ.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            return Ok("sss");
        }
    }
}
