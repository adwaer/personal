using System;
using System.Data.Entity;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using CostEffectiveCode.Autofac;
using CostEffectiveCode.Common;
using CostEffectiveCode.Common.Logger;
using CostEffectiveCode.Common.Scope;
using CostEffectiveCode.Domain.Cqrs;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.EntityFramework6;
using CostEffectiveCode.NLog;
using Personal.Domain.Entities;
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

            //builder
            //    .Register(x => Container)
            //    .As<IDiContainer>()
            //    .SingleInstance();

            builder
                .RegisterType<DependencyResolverDiContainer>()
                .As<IDiContainer>()
                .SingleInstance();

            builder.RegisterType<MyCtx>()
                .AsSelf()
                .As<DbContext>()
                .As<IDataContext>()
                .As<IUnitOfWork>()
                .As<ILinqProvider>()
                .InstancePerLifetimeScope();

            //builder.RegisterType<DiContainerScope<IUnitOfWork>>()
            //    .As<IScope<IUnitOfWork>>()
            //    .SingleInstance();

            //builder.RegisterType<DiContainerScope<IDataContext>>()
            //    .As<IScope<IDataContext>>()
            //    .SingleInstance();
            
            builder.RegisterType<CommandQueryFactory>()
                .AsImplementedInterfaces()
                .InstancePerRequest();
            
            builder.RegisterGeneric(typeof(CreateEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterGeneric(typeof(DeleteEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterType<CommitCommand>()
                .InstancePerDependency();

            builder
                .RegisterType<ExpressionQuery<Text>>()
                .As<IQuery<Text, IExpressionSpecification<Text>>>()
                .InstancePerDependency();

            builder
                .RegisterGeneric(typeof(ExpressionSpecification<>))
                .As(typeof(IExpressionSpecification<>));

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
                .InstancePerRequest();
            
            builder.RegisterApiControllers(Assembly.GetCallingAssembly());
            
            return builder.Build();
        }
    }
}
