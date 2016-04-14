using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MQ.WebApi.Controllers
{
    public class LocationController : ApiController
    {
        [HttpGet]
        [Route("ip/location/{ip}")]
        public async Task<IHttpActionResult> LocationByIp(string ip)
        {
            return Ok(ip);
        }

        [HttpGet]
        [Route("city/locations/{city}")]
        public async Task<IHttpActionResult> LocationByCity(string city)
        {
            return Ok(city);
        }
    }
}
