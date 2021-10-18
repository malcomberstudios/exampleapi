using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.CacheLayer;
using CocktailApi.Lib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CocktailController : ControllerBase
    {
        private readonly CocktailCacheLayer _cocktailCacheLayer;
        private readonly ILogger<CocktailController> _logger;

        public CocktailController(
            ILogger<CocktailController> logger,
            CocktailCacheLayer cocktailCacheLayer)
        {
            _logger = logger;
            _cocktailCacheLayer = cocktailCacheLayer;
        }

        /// <summary>
        /// List of all popular drinks
        /// </summary>
        /// <returns>A List of all popular drinks</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drink>>> Get()
        {
            try
            {
                return (await _cocktailCacheLayer.GetPopularDrinks())
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.Message);
                return StatusCode(503);
            }
        }


        /// <summary>
        /// Get All the ingredients of the cocktail system
        /// </summary>
        /// <returns>A List of Cocktail ingredients</returns>
        [HttpGet("Ingredients")]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            try
            {
                return (await _cocktailCacheLayer.GetIngredients()).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.Message);
                return StatusCode(503);
            }

            ;
        }

        [HttpGet("search/{ingredient}")]
        public async Task<ActionResult<IEnumerable<Drink>>> SearchIngredientForDrinks(string ingredient)
        {
            // Validate Ingredient
            var ingredients = await _cocktailCacheLayer.GetIngredients();
            if (!ingredients.Any(i => string.Equals(i.Name, ingredient, StringComparison.CurrentCultureIgnoreCase)))
            {

                return StatusCode(400);
            }


            try
            {
                return (await _cocktailCacheLayer.SearchIngredientsForDrinks(ingredient)).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("{Message}", e.Message);
                return StatusCode(503);
            }
        }
    }
}