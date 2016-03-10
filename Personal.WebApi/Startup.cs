using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Personal.WebApi.Config;

[assembly: OwinStartup(typeof(Personal.WebApi.Startup))]
namespace Personal.WebApi
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            var container = IocConfig.Configure();
            ConfigureApp(container, app);
            
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Service is online.");
            });
        }


        private void ConfigureApp(IContainer container, IAppBuilder app)
        {
            HttpConfiguration = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(container)
            };

            HttpConfiguration.Formatters.Remove(HttpConfiguration.Formatters.XmlFormatter);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(HttpConfiguration);

            RouteConfig.Register(HttpConfiguration);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(HttpConfiguration);
        }
    }
}
