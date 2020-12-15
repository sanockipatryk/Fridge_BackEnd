using System;
using System.Collections.Generic;
using System.Linq;
using Fridge_BackEnd.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Fridge_BackEnd.Features.IngredientCategories
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientCategoriesController : ControllerBase
    {
        private readonly FridgeContext _db;

        public IngredientCategoriesController(FridgeContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ingredientCategories = await _db.IngredientCategories.Select(ic => new IngredientCategoriesListViewModel
            {
                Id = ic.Id,
                Name = ic.Name,
            }).OrderBy(ic => ic.Name).ToListAsync();
            return Ok(ingredientCategories);
        }
    }
}
