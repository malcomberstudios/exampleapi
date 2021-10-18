using System.Collections.Generic;
using Newtonsoft.Json;

namespace CocktailApi.Lib
{
    public class ListOfDrinks
    {
        [JsonProperty("drinks")]
        public List<Drink> Drinks { get; set; }
    }
}