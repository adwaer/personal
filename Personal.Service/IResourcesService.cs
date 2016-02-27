using System.Collections;

namespace Personal.Service
{
    public interface IResourcesService
    {
        IEnumerable Get(string name);
    }
}
