using System.Collections.Generic;
using Newtonsoft.Json;

namespace CocktailApi.Lib
{
    public class Ingredients
    {
        [JsonProperty("drinks")] public IEnumerable<Ingredient> Drinks { get; set; }
    }
}