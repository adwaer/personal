using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Personal.Service;
using Resources;

namespace Personal.Resource
{
    public class ResourcesService : IResourcesService
    {
        private readonly ConcurrentDictionary<ResourceCache, ResourceSet> _resourceDictionary;
        public ResourcesService()
        {
            _resourceDictionary = new ConcurrentDictionary<ResourceCache, ResourceSet>();
        }

        public IEnumerable Get(string name)
        {
            return GetResourceSet(name);
        }

        public void Update(string key, string value)
        {
            throw new NotImplementedException();
        }

        public ResourceSet GetResourceSet(string id)
        {
            var resourceCache = ResourceCache.GetInstance(id);
            ResourceSet resourceSet;
            if (_resourceDictionary.TryGetValue(resourceCache, out resourceSet))
            {
                return resourceSet;
            }


            string typeName = GetResourceNames()
                .FirstOrDefault(resourceName => resourceName.Contains($"{id}.resources"));
            if (string.IsNullOrEmpty(typeName))
            {
                throw new ArgumentException("Resource not found");
            }

            typeName = typeName.Substring(0, typeName.IndexOf(".resources", StringComparison.Ordinal));
            var type = GetResourceManager(typeName);
            if (type == null)
            {
                throw new ArgumentException("Resource not found");
            }

            resourceSet = type.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            _resourceDictionary[resourceCache] = resourceSet;

            return resourceSet;
        }

        private static string[] _resourceNames;
        private static string[] GetResourceNames()
        {
            return _resourceNames ?? (_resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames());
        }

        private static ResourceManager GetResourceManager(string name)
        {
            return new ResourceManager(name, typeof(Layout).Assembly);
        }

        
    }
}
