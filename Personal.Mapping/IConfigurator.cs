using AutoMapper;
using CostEffectiveCode.Domain.Ddd.Entities;

namespace Personal.Mapping
{
    public interface IConfigurator
    {
        IMappingExpression<TFrom, TTo> Configure<TFrom, TTo>();

        IMappingExpression<TFrom, TTo> ConfigureToEntity<TFrom, TTo>()
            where TTo : class, IEntity;
    }
}
