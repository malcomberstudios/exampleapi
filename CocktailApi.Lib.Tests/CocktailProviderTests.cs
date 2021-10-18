using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Test.Lib;
using Xunit;

namespace CocktailApi.Lib.Tests
{
    public class CocktailProviderTests
    {

        private CocktailProviderConfiguration _config = new()
        {
            CocktailApiKey = "MYAPIKEY",
            CocktailApiUri = "example.com"
        };


        [Fact]
        public async Task GetIngredientsTest()
        {
            var resp = (await GetCocktailProvider(_config).GetIngredients()).ToList();
            Assert.NotEmpty(resp);

            var first = resp.First();
            Assert.True(!string.IsNullOrWhiteSpace(first.Name));
            
            var badProvider = GetCocktailProvider(new CocktailProviderConfiguration
            {
                CocktailApiUri = _config.CocktailApiUri,
                CocktailApiKey = $"{_config.CocktailApiKey}_INVALIDAPIKEY"
            });

            await Assert.ThrowsAsync<HttpRequestException>(() => badProvider.GetIngredients());
        }

        [Fact]
        public async Task GetPopularDrinks()
        {
            var resp = (await GetCocktailProvider(_config).GetPopularDrinks()).ToList();
            Assert.NotEmpty(resp);

            var first = resp.First();

            Assert.IsType<string>(first.DateModified);
            Assert.IsType<string>(first.IdDrink);
            Assert.IsType<string>(first.StrAlcoholic);
            Assert.IsType<string>(first.StrDrink);

            var badProvider = GetCocktailProvider(new CocktailProviderConfiguration
            {
                CocktailApiUri = _config.CocktailApiUri,
                CocktailApiKey = $"{_config.CocktailApiKey}_INVALIDAPIKEY"
            });

            await Assert.ThrowsAsync<HttpRequestException>(() => badProvider.GetPopularDrinks());
        }

        [Fact]
        public async Task SearchForDrinks()
        {
            var resp = await GetCocktailProvider(_config).SearchDrinksWhichUseIngredient("vodka");
            Assert.NotEmpty(resp);
            
            var badProvider = GetCocktailProvider(new CocktailProviderConfiguration
            {
                CocktailApiUri = _config.CocktailApiUri,
                CocktailApiKey = $"{_config.CocktailApiKey}_INVALIDAPIKEY"
            });

            await Assert.ThrowsAsync<HttpRequestException>(() => badProvider.SearchDrinksWhichUseIngredient("vodka"));
        }

        private ICocktailProvider GetCocktailProvider(CocktailProviderConfiguration cocktailProviderConfiguration)
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.TestServiceProvider(cocktailProviderConfiguration);

            return serviceProvider.GetService<CocktailProvider>();

        }
    }
}