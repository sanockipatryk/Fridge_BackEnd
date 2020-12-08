using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fridge_BackEnd.Data;
using Fridge_BackEnd.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fridge_BackEnd.Features.Recipes
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly FridgeContext _db;

        public RecipesController(FridgeContext db)
        {
            _db = db;
        }

        [HttpGet("getUserRecipes")]
        [Authorize]
        public async Task<IActionResult> GetUserFridges()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var userRecipes = await _db.Recipes.Where(f => f.UserId == appUser.Id).Select(i => new RecipesListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                DateCreated = i.DateCreated,
                DateModified = i.DateModified,
                CookingTime = i.CookingTime
            }).ToListAsync();

            return Ok(userRecipes);
        }

        [HttpPost("createRecipe")]
        [Authorize]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var newRecipe = new Recipe
            {
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
                CookingTime = model.CookingTime,
                UserId = appUser.Id,
            };

            _db.Recipes.Add(newRecipe);
            await _db.SaveChangesAsync();

            //var newRecipeIngredients = new RecipeIngredient
            //{
                
            //}

            return Ok();
        }
    }
}
