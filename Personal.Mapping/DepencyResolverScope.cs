using CostEffectiveCode.Common;

namespace Personal.Mapping
{
    public class DepencyResolverScope<T> : IScope<T>
    {
        private readonly IDiContainer _container;

        public DepencyResolverScope(IDiContainer container)
        {
            _container = container;
        }

        public T Instance => _container.Resolve<T>();
    }
}