using System.Net.Http;
using Microsoft.Owin.Hosting;
using Personal.WebApi;

namespace Personal.Console
{
    class Program
    {
        private const string Uri = "http://localhost:7777/";
        static void Main()
        {
            System.Console.WriteLine("Starting");

            var start = WebApp.Start<Startup>(Uri);
            //using (WebApp.Start<Startup>(Uri))
            //{
            //    var client = new HttpClient();

            //    var response = client.GetAsync($"{Uri}api/values").Result;

            //    System.Console.WriteLine(response);
            //    System.Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            //}

            System.Console.ReadLine();
        }


    }
}
