using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal.Resource
{
    public class ResourceCache
    {
        public readonly CultureInfo Culture;
        public readonly string Id;

        private ResourceCache(string id, CultureInfo culture)
        {
            Culture = culture;
            Id = id;
        }

        public static ResourceCache GetInstance(string id)
        {
            return new ResourceCache(id, CultureInfo.CurrentUICulture);
        }

        private bool Equals(ResourceCache other)
        {
            return Equals(Culture, other.Culture) && string.Equals(Id, other.Id);
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
                return ((Culture?.GetHashCode() ?? 0) * 397) ^ (Id?.GetHashCode() ?? 0);
            }
        }
    }
}
