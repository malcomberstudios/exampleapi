using System.Net;
using System.Threading.Tasks;
using Api.CacheLayer;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Test.Lib;
using Xunit;

namespace Api.Tests
{
    public class CocktailControllerTests
    {
        private static CocktailController CocktailController
        {
            get
            {
                var services = new ServiceCollection();
                var serviceProvider = services.TestServiceProvider();

                var cocktailCacheLayer = serviceProvider.GetService<CocktailCacheLayer>();
                var logger = NullLogger<CocktailController>.Instance;

                var cocktailController = new CocktailController(logger, cocktailCacheLayer);
                return cocktailController;
            }
        }

        [Fact]
        public async Task GetPopularDrinksTest()
        {

            var controller = CocktailController;

            var result = await controller.Get();
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task GetIngredientsTest()
        {
            var controller = CocktailController;

            var result = await controller.GetIngredients();
            Assert.NotEmpty(result.Value);
        }
        
        [Fact]
        public async Task SearchIngredientsDrinksTest()
        {
            var controller = CocktailController;
            var result = await controller.SearchIngredientForDrinks("vodka");
            Assert.NotEmpty(result.Value);

            result = await controller.SearchIngredientForDrinks("notAnIngredient");
            Assert.Equal(400, ((StatusCodeResult)result.Result).StatusCode);
        }
    }
}