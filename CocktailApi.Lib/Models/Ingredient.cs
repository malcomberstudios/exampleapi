using Newtonsoft.Json;

namespace CocktailApi.Lib
{
    public class Ingredient
    {
        [JsonProperty("strIngredient1")] public string Name { get; set; }
    }
}