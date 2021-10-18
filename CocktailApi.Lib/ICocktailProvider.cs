using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailApi.Lib
{
    public interface ICocktailProvider
    {
        Task<IEnumerable<Ingredient>> GetIngredients();
        Task<IEnumerable<Drink>> GetPopularDrinks();
        Task<IEnumerable<Drink>> SearchDrinksWhichUseIngredient(string ingredient);
    }
}