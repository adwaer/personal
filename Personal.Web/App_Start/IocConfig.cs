using System.Data.Entity;
using System.Reflection;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using CostEffectiveCode.Common;
using CostEffectiveCode.Domain.Cqrs;
using CostEffectiveCode.Domain.Cqrs.Commands;
using CostEffectiveCode.Domain.Ddd.UnitOfWork;
using CostEffectiveCode.EntityFramework6;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Personal.Domain.Entities;
using Personal.Mapping;
using Personal.Schema;
using Personal.User;

namespace Personal.Web
{
    public static class IocConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SignInManager<Customer, int>>()
                .AsSelf()
                .InstancePerLifetimeScope();

            //builder.Register<UserManager<User, Guid>>(c => SdkUserManager.Get(c.Resolve<ScCtx>()))
            //    .InstancePerLifetimeScope();

            builder.RegisterType<MyCtx>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MyUserManager>()
                .As<UserManager<Customer, int>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MyUserStore>()
                .As<IUserStore<Customer, int>>()
                .InstancePerLifetimeScope();

            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
                .InstancePerLifetimeScope();
            
            builder.RegisterType<AutoMapperWrapper>().AsImplementedInterfaces().SingleInstance();

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
                .InstancePerRequest();

            builder.RegisterType<DependencyResolverDiContainer>()
                .As<IDiContainer>()
                .SingleInstance();

            builder.RegisterGeneric(typeof (CreateEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterGeneric(typeof (DeleteEntityCommand<>))
                .InstancePerDependency();

            builder.RegisterType<CommitCommand>()
                .InstancePerDependency();

            builder.RegisterGeneric(typeof (ExpressionQuery<>))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            builder.RegisterType<NLogLogger>().As<ILogger>();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            return builder.Build();
        }

    }
}