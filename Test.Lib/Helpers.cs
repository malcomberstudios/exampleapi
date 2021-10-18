using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Api.providers;
using CocktailApi.Lib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Contrib.HttpClient;

namespace Test.Lib
{
    public static class Helpers
    {
        
        private static  string _ingredientsData;
        private static  string _popularDrinksData;
        private static string _searchData;

        private static CocktailProviderConfiguration _config = new CocktailProviderConfiguration
        {
            CocktailApiKey = "MYAPIKEY",
            CocktailApiUri = "example.com"
        };
        
        public static IServiceProvider TestServiceProvider(this IServiceCollection serviceCollection, CocktailProviderConfiguration cocktailProviderConfiguration = null)
        {
            _ingredientsData = File.ReadAllText("IngredientsData.json");
            _popularDrinksData = File.ReadAllText("PopularDrinks.json");
            _searchData = File.ReadAllText("SearchData.json");

            var config = cocktailProviderConfiguration ?? _config;

            var services = new ServiceCollection();
            services.AddTransient(builder => config);
            services.AddTransient<ILogger<CocktailProvider>>(builder => NullLogger<CocktailProvider>.Instance);
            services.AddTransient<CacheProvider>();

            
            services.AddHttpClient("rapidapi-cocktail")
                .ConfigurePrimaryHttpMessageHandler(() => GetMessageHandler(config).Object);
            
            
            services.AddMemoryCache();
            services.AddCocktailServices();
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;

        }


        public static Mock<HttpMessageHandler> GetMessageHandler(CocktailProviderConfiguration cocktailProviderConfiguration)
        {
            var handler = new Mock<HttpMessageHandler>();
            var httpFactory = handler.CreateClientFactory();
            
            Mock.Get(httpFactory).Setup(x => x.CreateClient("rapidapi-cocktail"))
                .Returns(() =>
                {
                    var client = handler.CreateClient();
                    return client;
                });
            
            SetUpHandler(handler, cocktailProviderConfiguration, "list.php?i=list", _ingredientsData);
            SetUpHandler(handler, cocktailProviderConfiguration, "popular.php", _popularDrinksData);
            SetUpHandler(handler, cocktailProviderConfiguration, "filter.php?i=vodka", _searchData);

            return handler;
        }
        
        private static void SetUpHandler(Mock<HttpMessageHandler> handler,
            CocktailProviderConfiguration cocktailProviderConfiguration, string url, string responseData)
        {
            handler
                .SetupRequest(HttpMethod.Get, $"https://{cocktailProviderConfiguration.CocktailApiUri}/{url}")
                .ReturnsResponse(HttpStatusCode.OK, configure: response =>
                {
                    var keys = response.RequestMessage?.Headers.GetValues("x-rapidapi-key");
                    var keyValid = keys != null && keys.Any(k => k == "MYAPIKEY");

                    if (keyValid)
                    {
                        response.Content = new StringContent(responseData, Encoding.UTF8, "application/json");
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.Unauthorized;
                    }
                });
        }
    }
}