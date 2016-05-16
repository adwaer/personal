using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Bets.Http
{
    public class HtmlDom
    {
        private readonly HtmlDocument _doc;
        public HtmlDom()
        {
            _doc = new HtmlDocument();
        }

        public async Task Load(string url)
        {
            HttpClient client = new HttpClient();
            var html = await client.GetStringAsync(url);
            _doc.LoadHtml(html);
        }

        public HtmlNode GetById(string id, HtmlNode node = null)
        {
            return GetByType("id", id, node)
                .FirstOrDefault();
        }

        public HtmlNodeCollection GetByClass(string className, HtmlNode node = null)
        {
            return GetByType("class", className, node);
        }

        public HtmlNodeCollection GetByType(string type, string selector, HtmlNode node = null, string tag = "*")
        {
            return GetBySelector($"//{tag}[@{type}=\"{selector}\"]", node);
        }

        public HtmlNodeCollection GetByTag(string tag, HtmlNode node)
        {
            return GetBySelector($"//{tag}", node);
        }

        public HtmlNodeCollection GetBySelector(string selector, HtmlNode node = null)
        {
            var singleNode = (node ?? _doc.DocumentNode)
                .SelectNodes(selector);

            return singleNode;
        }
    }
}
