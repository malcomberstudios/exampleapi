using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.CacheLayer;
using CocktailApi.Lib;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Test.Lib;
using Xunit;

namespace Api.Tests
{
    public class CocktailCacheLayerTests
    {
        [Fact]
        public async Task TestCacheLayer()
        {
            var services = new ServiceCollection();
            var serviceProvider = services.TestServiceProvider();

            var cacheLayer = serviceProvider.GetService<CocktailCacheLayer>();
            
            Assert.NotNull(cacheLayer);

            var ingredientsCore = (await cacheLayer.GetIngredients()).ToList();
            Assert.NotEmpty(ingredientsCore);

            var popularDrinksCore =  (await cacheLayer.GetPopularDrinks()).ToList();
            Assert.NotEmpty(popularDrinksCore);

            var searchResultsCore = (await cacheLayer.SearchIngredientsForDrinks("vodka")).ToList();
            Assert.NotEmpty(searchResultsCore);

            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            Assert.True(memoryCache.TryGetValue("_PopularDrinks", out List<Drink> popularDrinks));

            Assert.True(memoryCache.TryGetValue("_GetIngredients", out List<Ingredient> ingredients));

            Assert.True(memoryCache.TryGetValue("_Search_vodka", out List<Drink> searchResults));
            
            Assert.Equal(ingredientsCore, ingredients);
            Assert.Equal(popularDrinksCore, popularDrinks);
            Assert.Equal(searchResultsCore, searchResults);

        }
    }
}