using System.Threading.Tasks;
using Api.providers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Test.Lib;
using Xunit;

namespace Api.Tests
{
    public class CacheProviderTests
    {
        [Fact]
        public void TryGetValue()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.TestServiceProvider();

            var cache = serviceProvider.GetService<IMemoryCache>();

            var cacheProvider = new CacheProvider(cache);

            var returnValue = "Hello World!";
            var key = "my_awesome_key";
            var result = cacheProvider.TryGetValue(key, () => Task.FromResult(returnValue));
            
            Assert.Equal(returnValue, result.Result);

            var valueOfCache = "";
            Assert.True(cache.TryGetValue(key, out valueOfCache));
            Assert.Equal(returnValue, valueOfCache);
            
        }
    }
}