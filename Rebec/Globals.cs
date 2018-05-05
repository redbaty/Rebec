using LazyCache;

namespace Rebec
{
    internal static class Globals
    {
        public static IAppCache Cache { get; } = new CachingService();
    }
}