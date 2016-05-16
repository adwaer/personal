using System.Threading.Tasks;
using System.Web.Http;

namespace Bets.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        public async Task<IHttpActionResult> Post(string login, string password)
        {
            return Ok(login);
        }
    }
}
