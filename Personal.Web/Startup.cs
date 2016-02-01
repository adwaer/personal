using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using Personal.Mapping;
using Personal.Web.Models;

[assembly: OwinStartup(typeof(Personal.Web.Startup))]

namespace Personal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var container = IocConfig.Configure();
            ConfigureApp(container, app);
        }

        private void ConfigureApp(IContainer container, IAppBuilder app)
        {
            IConfigurator configurator = container.Resolve<IConfigurator>();

            new ViewModelConfigurator(configurator).Start();

            ConfigureWebApi(app, container);
            ConfigureMvc(app, container);
        }

        private static void ConfigureWebApi(IAppBuilder app, IContainer container)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new AutofacWebApiDependencyResolver(container)
            };

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);

            WebApiConfig.Register(config);
        }

        // ReSharper disable once UnusedParameter.Local
        private static void ConfigureMvc(IAppBuilder app, IContainer container)
        {
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
