using EFCoreSecondLevelCacheInterceptor;

namespace CloudDataProtection.Services.EmployeeService.Tests.Mocks
{
    public class NullEFCacheServiceProvider : IEFCacheServiceProvider
    {
        public void ClearAllCachedEntries()
        {
        }

        public EFCachedData GetValue(EFCacheKey cacheKey, EFCachePolicy cachePolicy)
        {
            return new EFCachedData();
        }

        public void InsertValue(EFCacheKey cacheKey, EFCachedData value, EFCachePolicy cachePolicy)
        {
        }

        public void InvalidateCacheDependencies(EFCacheKey cacheKey)
        {
        }
    }
}