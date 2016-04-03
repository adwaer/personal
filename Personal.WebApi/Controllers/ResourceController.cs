using System.Web.Http;
using Personal.Service;
using Personal.WebApi.Attribute;

namespace Personal.WebApi.Controllers
{
    [Auth]
    public class ResourceController : ApiController
    {
        private readonly IResourcesService _resourcesService;

        public ResourceController(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        public IHttpActionResult Get(string id)
        {
            return Ok(_resourcesService.Get(id));
        }

        [Authorize]
        public IHttpActionResult Put(string id, string value)
        {
            _resourcesService.Update(id, value);
            return Ok();
        }
    }
}
