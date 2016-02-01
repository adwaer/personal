using System.Web.Mvc;
using CostEffectiveCode.Common;

namespace Personal.Mapping
{
    public class DependencyResolverDiContainer : IDiContainer
    {
        public T Resolve<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }
}