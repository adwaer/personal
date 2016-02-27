using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using Personal.Service;

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

            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException("Resource not found");
            }

            resourceSet = MakeResourceJson(type);
            _resourceDictionary[resourceCache] = resourceSet;

            return resourceSet;
        }

        private static ResourceSet MakeResourceJson(Type type)
        {
            PropertyInfo resourceManagerPty = type.GetProperty("ResourceManager");
            var rm = (ResourceManager)resourceManagerPty.GetValue(null);

            return rm.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            //var json = JsonConvert.SerializeObject(resourceSet);

            //ResourceDictionary.TryAdd(resourceCache, json);

            //return json;
        }

        private static string[] _resourceNames;
        private static string[] GetResourceNames()
        {
            return _resourceNames ?? (_resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames());
        }

        private class ResourceCache
        {
            private readonly CultureInfo _culture;
            private readonly string _id;

            private ResourceCache(string id, CultureInfo culture)
            {
                _culture = culture;
                _id = id;
            }

            public static ResourceCache GetInstance(string id)
            {
                return new ResourceCache(id, CultureInfo.CurrentUICulture);
            }

            private bool Equals(ResourceCache other)
            {
                return Equals(_culture, other._culture) && string.Equals(_id, other._id);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((ResourceCache)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_culture?.GetHashCode() ?? 0) * 397) ^ (_id?.GetHashCode() ?? 0);
                }
            }
        }
    }
}
