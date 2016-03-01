using System.Data.Entity;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.EntityFramework6;
using Personal.Mapping;
using Personal.Resource;
using Personal.Schema;
using Personal.Service;

namespace Personal.WebApi.Config
{
    public class IocConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MyCtx>()
                .AsSelf()
                .As<DbContext>()
                .As<IDataContext>()
                .As<IUnitOfWork>()
                .As<ILinqProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DepencyResolverScope<IUnitOfWork>>()
                .As<IScope<IUnitOfWork>>()
                .SingleInstance();

            builder.RegisterType<DepencyResolverScope<IDataContext>>()
                .As<IScope<IDataContext>>()
                .SingleInstance();

            builder.RegisterType<DepencyResolverScope<IQueryFactory>>()
                .As<IScope<IQueryFactory>>()
                .SingleInstance();

            builder.RegisterType<CommandQueryFactory>()
                .AsImplementedInterfaces()
                .InstancePerRequest();

            builder.RegisterType<DependencyResolverDiContainer>()
                .As<IDiContainer>()
                .SingleInstance();

            builder.RegisterGeneric(typeof(CreateEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterGeneric(typeof(DeleteEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterType<CommitCommand>()
                .InstancePerDependency();

            builder.RegisterGeneric(typeof(ExpressionQuery<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<NLogLogger>()
                .As<ILogger>()
                .InstancePerRequest();

            builder
                .RegisterType<AutoMapperWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .RegisterType<DbResourcesService>()
                .As<IResourcesService>()
                .SingleInstance();

            builder.RegisterApiControllers(Assembly.GetCallingAssembly());

            return builder.Build();
        }
    }
}
