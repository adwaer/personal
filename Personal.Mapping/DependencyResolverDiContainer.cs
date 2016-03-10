using System;
using Autofac;
using Autofac.Core.Lifetime;
using CostEffectiveCode.Common;

namespace Personal.Mapping
{
    public class DependencyResolverDiContainer : IDiContainer
    {
        private readonly Func<IContainer> _container;

        public DependencyResolverDiContainer(Func<IContainer> container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            using (var scope = _container().BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag))
            {
                return scope.Resolve<T>();
            }
        }
    }
}