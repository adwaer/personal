using System.Data.Entity;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Bets.Cqrs.Query;
using Bets.Dal;

namespace Bets.WebApi.Config
{
    public class IocConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder
                .RegisterType<DefaultCtx>()
                .As<DbContext>()
                .InstancePerRequest();

            builder
                .RegisterType<BetsQuery>()
                .AsSelf()
                .InstancePerRequest();


            builder.RegisterApiControllers(Assembly.GetCallingAssembly());

            return builder.Build();
        }
    }
}
