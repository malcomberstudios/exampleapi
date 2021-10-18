using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.providers;
using CocktailApi.Lib;

namespace Api.CacheLayer
{
    public class CocktailCacheLayer
    {
        private readonly CacheProvider _cache;
        private readonly CocktailProvider _cocktailProvider;

        public CocktailCacheLayer(CacheProvider cache, CocktailProvider cocktailProvider)
        {
            _cache = cache;
            _cocktailProvider = cocktailProvider;
        }


        public Task<IEnumerable<Drink>> GetPopularDrinks()
        {
            return _cache.TryGetValue("_PopularDrinks",
                () => _cocktailProvider.GetPopularDrinks());
        }

        public Task<IEnumerable<Ingredient>> GetIngredients()
        {
            return _cache.TryGetValue("_GetIngredients",
                () => _cocktailProvider.GetIngredients());
        }

        public Task<IEnumerable<Drink>> SearchIngredientsForDrinks(string ingredient)
        {
            return _cache.TryGetValue($"_Search_{ingredient.ToLower()}",
                () => _cocktailProvider.SearchDrinksWhichUseIngredient(ingredient));
        } 
    }
}