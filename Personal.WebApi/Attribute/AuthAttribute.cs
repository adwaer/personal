using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Personal.User;

namespace Personal.WebApi.Attribute
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IAuthManager service = (IAuthManager)Startup
                .HttpConfiguration
                .DependencyResolver
                .GetService(typeof(IAuthManager));

            var currentClient = service.GetCurrentClient();
            if (currentClient == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                return base.OnAuthorizationAsync(actionContext, cancellationToken);
            }

            return Task.FromResult<object>(null);
        }
    }
}
