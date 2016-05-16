using System.Threading;

namespace SN.Business
{
    public static class Singleton<T>
    {   
        // ReSharper disable once StaticMemberInGenericType it's ok
        static bool _initialized;
        // ReSharper disable once StaticMemberInGenericType it's ok
        static object _sync;

        static T _instance;

        static Singleton()
        {
            _sync = new object();
            _initialized = false;
        }

        public static T Instance => LazyInitializer.EnsureInitialized(ref _instance, ref _initialized, ref _sync);
    }
}
