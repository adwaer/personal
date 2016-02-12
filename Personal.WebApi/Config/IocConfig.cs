using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Personal.WebApi.Controllers;

namespace Personal.WebApi.Config
{
    public class IocConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterType<SampleProjectDbContext>()
            //    .AsSelf()
            //    .As<DbContext>()
            //    .As<IDataContext>()
            //    .As<IUnitOfWork>()
            //    .As<ILinqProvider>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<DepencyResolverScope<IUnitOfWork>>()
            //    .As<IScope<IUnitOfWork>>()
            //    .SingleInstance();

            //builder.RegisterType<DepencyResolverScope<IDataContext>>()
            //    .As<IScope<IDataContext>>()
            //    .SingleInstance();

            //builder.RegisterType<CommandQueryFactory>()
            //    .AsImplementedInterfaces()
            //    .InstancePerRequest();

            //builder.RegisterType<DiContainer>()
            //    .As<IDiContainer>()
            //    .SingleInstance();

            //builder.RegisterGeneric(typeof(CreateEntityCommand<>))
            //    .InstancePerDependency();

            //builder.RegisterGeneric(typeof(DeleteEntityCommand<>))
            //    .InstancePerDependency();

            //builder.RegisterType<CommitCommand>()
            //    .InstancePerDependency();

            //builder.RegisterGeneric(typeof(ExpressionQuery<>))
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();

            //builder.RegisterType<ConsoleLogger>()
            //    .As<ILogger>()
            //    .InstancePerRequest();

            //builder
            //    .RegisterType<AutoMapperWrapper>()
            //    .AsImplementedInterfaces()
            //    .SingleInstance();

            builder.RegisterApiControllers(Assembly.GetEntryAssembly());
            return builder.Build();
        }
    }
}
