using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CocktailApi.Lib
{
    /// <summary>
    /// Used to provide access to the Cocktail API
    /// </summary>
    public class CocktailProvider : ICocktailProvider
    {
        private readonly CocktailProviderConfiguration _configuration;
        private readonly ILogger<CocktailProvider> _logger;
        private readonly HttpClient _client;

        /// <summary>
        /// Construct the cocktail provider
        /// </summary>
        /// <param name="configuration">Valid configuration for accessing the cocktail api</param>
        /// <param name="clientFactory">Needed for building a new Http Clients.</param>
        /// <param name="logger">Standard Logger</param>
        /// <remarks>We use HttpClientFactory to stop memory leaks with ASP Core applications</remarks>
        public CocktailProvider(CocktailProviderConfiguration configuration, IHttpClientFactory clientFactory,
            ILogger<CocktailProvider> logger)
        {
            _configuration = configuration;
            _client = clientFactory.CreateClient("rapidapi-cocktail");
            _logger = logger;
        }


        /// <summary>
        /// Gathers a list of all the ingredients.
        /// </summary>
        /// <returns>List of ingredients within the database</returns>
        /// <exception cref="CouldNotGetDrinksException"></exception>
        public async Task<IEnumerable<Ingredient>> GetIngredients()
        {
            _logger.LogInformation("Gathering list of ingredients");
            var response = await _client.SendAsync(GetRequestMessage("list.php?i=list"));
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var ingredients = JsonConvert.DeserializeObject<Ingredients>(body);
            return ingredients == null ? new List<Ingredient>() : ingredients.Drinks;
        }

        /// <summary>
        /// Get's a list of popular drinks. 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CouldNotGetIngredientsException"></exception>
        public async Task<IEnumerable<Drink>> GetPopularDrinks()
        {
            var response = await _client.SendAsync(GetRequestMessage("popular.php"));
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var popularDrinks = JsonConvert.DeserializeObject<PopularDrinks>(body);
            return popularDrinks == null ? new List<Drink>() : popularDrinks.Drinks;
        }

        /// <summary>
        /// Searches for a drink given a specific ingredient.
        /// </summary>
        /// <param name="ingredient">Ingredient name</param>
        /// <returns>A list of drinks that uses the ingredient</returns>
        /// <exception cref="CouldNotGetSearchException"></exception>
        /// <remarks>
        /// Does not validate the ingredient is a valid ingredient.
        /// Invalid ingredient name will result in using a request which will cost. Validate before using, use
        /// <see cref="GetIngredients"/> to validate.
        /// </remarks>
        public async Task<IEnumerable<Drink>> SearchDrinksWhichUseIngredient(string ingredient)
        {
            var response = await _client.SendAsync(GetRequestMessage($"filter.php?i={ingredient}"));
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var listOfDrinks = JsonConvert.DeserializeObject<ListOfDrinks>(body);
            return listOfDrinks == null ? new List<Drink>() : listOfDrinks.Drinks;
        }

        /// <summary>
        /// Used to create the Http Request Message with default headers.
        /// </summary>
        /// <param name="url">The URL to call, after the base URL</param>
        /// <returns>Ready to go request message</returns>
        private HttpRequestMessage GetRequestMessage(string url)
        {
            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://{_configuration.CocktailApiUri}/{url}"),
                Headers =
                {
                    { "x-rapidapi-host", _configuration.CocktailApiUri },
                    { "x-rapidapi-key", _configuration.CocktailApiKey },
                },
            };
        }
    }
}