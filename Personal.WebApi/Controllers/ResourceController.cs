using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web.Http;
using Personal.Service;

namespace Personal.WebApi.Controllers
{
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

    }
}
