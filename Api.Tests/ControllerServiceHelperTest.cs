using Api.providers;
using CocktailApi.Lib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Test.Lib;
using Xunit;

namespace Api.Tests
{
    public class ControllerServiceHelperTest
    {
        [Fact]
        public void TestExtension()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.TestServiceProvider();

            var cocktailProvider = serviceProvider.GetService<CocktailProvider>();
            Assert.NotNull(cocktailProvider);

            var cocktailCacheLater = serviceProvider.GetService<CocktailProvider>();
            Assert.NotNull(cocktailCacheLater);
        }
    }
}