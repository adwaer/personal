using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin.Logging;

namespace MQ.WebApi.Config
{
    public class IocConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetCallingAssembly());

            return builder.Build();
        }
    }
}
