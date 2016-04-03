using Autofac;
using CostEffectiveCode.Domain.Cqrs.Queries;
using CostEffectiveCode.Domain.Ddd.Specifications;
using CostEffectiveCode.EntityFramework6;
using Personal.Domain.Entities;

namespace Personal.Domain
{
    public static class CqrsIocConfig
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder
                .RegisterType<ExpressionQuery<Text>>()
                .As<IQuery<Text, IExpressionSpecification<Text>>>()
                .InstancePerDependency();

            builder
                .RegisterType<ExpressionQuery<Customer>>()
                .As<IQuery<Customer, IExpressionSpecification<Customer>>>()
                .InstancePerDependency();

            builder
                .RegisterType<ExpressionQuery<UserRole>>()
                .As<IQuery<UserRole, IExpressionSpecification<UserRole>>>()
                .InstancePerDependency();

        }
    }
}
