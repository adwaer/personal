using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Personal.Schema;
using Personal.User;
using Personal.WebApi.Config;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Owin.BasicAuthentication;

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
            ConfigureAuth(app);

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

            HttpConfiguration.SuppressDefaultHostAuthentication();
            HttpConfiguration.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            HttpConfiguration.Filters.Add(new HostAuthenticationFilter(BasicAuthenticationOptions.BasicAuthenticationType));
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(MyCtx.Create);
            app.CreatePerOwinContext(() => new MyUserManager(new MyUserStore(new MyCtx())));

            var basicAuthOptions = new BasicAuthenticationOptions("KMailWebManager", validationCallback);
            app.UseBasicAuthentication(basicAuthOptions);

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }

        private Task<IEnumerable<Claim>> validationCallback(string userName, string password)
        {
            using (DbContext dbContext = MyCtx.Create())
            using (MyUserStore userStore = new MyUserStore(dbContext))
            using (MyUserManager userManager = new MyUserManager(userStore))
            {
                var user = userManager.FindByName(userName);
                if (user == null)
                {
                    return null;
                }

                if (!userManager.CheckPassword(user, password))
                {
                    return null;
                }
                ClaimsIdentity claimsIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                return Task.FromResult(claimsIdentity.Claims);
            }
        }
    }
}
