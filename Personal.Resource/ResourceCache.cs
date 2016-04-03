using System;
using System.Globalization;
using System.Linq.Expressions;
using Personal.Domain.Entities;

namespace Personal.Resource
{
    public class ResourceCache
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

        public Expression<Func<Text, bool>> SearchPattern =>
            text => text.Culture == _culture.Name && text.Key == _id;

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
