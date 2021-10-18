using Newtonsoft.Json;

namespace CocktailApi.Lib
{
    public class Drink
    {
        [JsonProperty("idDrink")] public string IdDrink { get; set; }

        [JsonProperty("strDrink")] public string StrDrink { get; set; }
        
        [JsonProperty("strAlcoholic")] public string StrAlcoholic { get; set; }
        
        
        [JsonProperty("dateModified")] public string DateModified { get; set; }
    }
}