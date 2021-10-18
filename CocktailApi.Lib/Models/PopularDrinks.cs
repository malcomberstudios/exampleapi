using System.Collections.Generic;
using Newtonsoft.Json;

namespace CocktailApi.Lib
{
    public class PopularDrinks
    {
        [JsonProperty("drinks")] public List<Drink> Drinks { get; set; }
    }
}