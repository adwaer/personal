using CostEffectiveCode.Common;

namespace Personal.WebApi.Config
{
    public class DependencyResolverDiContainer : IDiContainer
    {
        public T Resolve<T>()
        {
            return (T)Startup.HttpConfiguration.DependencyResolver.GetService(typeof(T));
        }
    }
}
