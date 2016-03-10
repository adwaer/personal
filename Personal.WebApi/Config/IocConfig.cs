using System;
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
                .InstancePerLifetimeScope();

            builder.RegisterType<MyCtx>()
                .AsSelf()
                .As<IDataContext>()
                .As<IUnitOfWork>()
                .As<ILinqProvider>()
                .InstancePerRequest();

            builder.RegisterType<DepencyResolverScope<IUnitOfWork>>()
                .As<IScope<IUnitOfWork>>()
                .SingleInstance();

            builder.RegisterType<DepencyResolverScope<IDataContext>>()
                .As<IScope<IDataContext>>()
                .SingleInstance();

            builder.RegisterType<CommandQueryFactory>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

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
                .As<IResourcesService>();

            IContainer container = null;

            builder.RegisterApiControllers(Assembly.GetCallingAssembly());


            Func<IContainer> factory = () => container;
            builder.RegisterInstance(factory);

            container = builder.Build();
            return container;
        }
    }
}
