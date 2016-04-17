using System.Threading.Tasks;
using System.Web.Http;
using MQ.Cqrs.Query;
using MQ.Domain;

namespace MQ.WebApi.Controllers
{
    public class LocationController : ApiController
    {
        private readonly LocationByIpQuery _locationByIpQuery;
        private readonly LocationByCityQuery _locationByCityQuery;

        public LocationController(LocationByIpQuery locationByIpQuery,
             LocationByCityQuery locationByCityQuery)
        {
            _locationByIpQuery = locationByIpQuery;
            _locationByCityQuery = locationByCityQuery;
        }

        [HttpGet]
        [Route("ip/location/{ip}")]
        public async Task<IHttpActionResult> LocationByIp(string ip)
        {
            var location = await _locationByIpQuery
                .Execute(ip);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        [HttpGet]
        [Route("city/locations/{city}")]
        public async Task<IHttpActionResult> LocationByCity(string city)
        {
            var location = await _locationByCityQuery
                .Execute(city);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }
    }
}
