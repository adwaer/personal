using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using Personal.WebApi.Config;

[assembly: OwinStartup(typeof(Personal.WebApi.Startup))]
namespace Personal.WebApi
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder app)
        {
            var container = IocConfig.Configure();
            ConfigureApp(container, app);
            
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Servise is online.");
            });
        }

        private void ConfigureApp(IContainer container, IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(container)
            };

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            RouteConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
