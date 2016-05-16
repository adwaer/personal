using System.Threading.Tasks;
using System.Web.Http;
using Bets.Http;

namespace Bets.WebApi.Controllers
{
    public class ForecastController : ApiController
    {
        private const string ProUri = "http://vprognoze.ru/forecast/pro/";
        public async Task<IHttpActionResult> Get()
        {
            var htmlDom = new HtmlDom();
            await htmlDom.Load(ProUri);

            var content = htmlDom.GetById("dle-content");
            var news = htmlDom.GetByClass("news_boxing", content);

            foreach (var n in news)
            {
                var htmlNodeCollection = htmlDom.GetByTag("a", n);
            }

            return Ok(content);
        }
    }
}
