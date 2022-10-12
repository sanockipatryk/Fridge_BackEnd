using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fridge_BackEnd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fridge_BackEnd.Features.Ingredients
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly FridgeContext _db;

        public IngredientsController(FridgeContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ingredientCategories = await _db.Ingredients.Select(i => new IngredientListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                CategoryId = i.CategoryId,
                Category = i.IngredientCategory.Name
            }).OrderBy(i => i.Name).ToListAsync();
            return Ok(ingredientCategories);
        }
    }
}
