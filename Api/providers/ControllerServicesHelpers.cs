using Api.CacheLayer;
using CocktailApi.Lib;
using Microsoft.Extensions.DependencyInjection;

namespace Api.providers
{
    public static class ControllerServicesHelpers
    {
        public static IServiceCollection AddCocktailServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CocktailProvider>();
            serviceCollection.AddTransient<CocktailCacheLayer>();
            return serviceCollection;
        }
    }
}