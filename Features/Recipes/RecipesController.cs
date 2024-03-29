﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fridge_BackEnd.Data;
using Fridge_BackEnd.Data.Entities;
using Fridge_BackEnd.Features.RecipeIngredients;
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
        public async Task<IActionResult> GetUserRecipes()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var userRecipes = await _db.Recipes.Where(f => f.UserId == appUser.Id).Select(i => new RecipesListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                DateCreated = i.DateCreated,
                DateModified = i.DateModified,
                CookingTime = i.CookingTime,
                Ingredients = i.Ingredients.OrderBy(i => i.Quantity).Select(ing => new RecipeIngredients.RecipeIngredientsListViewModel
                {
                    IngredientId = ing.IngredientId,
                    CategoryId = ing.Ingredient.CategoryId,
                    Quantity = ing.Quantity
                }).ToList()
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

            var recipeIngredients = new List<RecipeIngredient>();
            model.Ingredients.ForEach(i => recipeIngredients.Add(new RecipeIngredient
            {
                RecipeId = newRecipe.Id,
                IngredientId = i.IngredientId,
                Quantity = i.Quantity
            }));

            _db.RecipeIngredients.AddRange(recipeIngredients);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editRecipe")]
        [Authorize]
        public async Task<IActionResult> EditRecipe([FromBody] EditRecipeViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentRecipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == model.RecipeId);
            if (currentRecipe != null)
            {
                if (appUser.Id != currentRecipe.UserId)
                    return Unauthorized();

                currentRecipe.Name = model.Name;
                currentRecipe.Description = model.Description;
                currentRecipe.CookingTime = model.CookingTime;

                _db.Recipes.Update(currentRecipe);
                await _db.SaveChangesAsync();

                var currentRecipeIngredients = await _db.RecipeIngredients.Where(ri => ri.RecipeId == model.RecipeId).ToListAsync();
                _db.RecipeIngredients.RemoveRange(currentRecipeIngredients);

                var recipeIngredients = new List<RecipeIngredient>();
                model.Ingredients.ForEach(i => recipeIngredients.Add(new RecipeIngredient
                {
                    RecipeId = model.RecipeId,
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity
                }));

                _db.RecipeIngredients.AddRange(recipeIngredients);
                await _db.SaveChangesAsync();

                return Ok();
            }
            else
                return NotFound();
        }

        [HttpDelete("deleteRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentRecipe = await _db.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            if (currentRecipe != null)
            {
                if (appUser.Id != currentRecipe.UserId)
                    return Unauthorized();

                var currentRecipeIngredients = await _db.RecipeIngredients.Where(ri => ri.RecipeId == id).ToListAsync();
                _db.RecipeIngredients.RemoveRange(currentRecipeIngredients);

                _db.Recipes.Remove(currentRecipe);
                await _db.SaveChangesAsync();

                return Ok();
            }
            else
                return NotFound();
        }

        [HttpPost("checkEnoughIngredients")]
        [Authorize]
        public async Task<IActionResult> CheckEnoughIngredients(UseRecipeViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentRecipe = await _db.Recipes.Include(r => r.Ingredients).ThenInclude(i => i.Ingredient).ThenInclude(i => i.IngredientCategory).FirstOrDefaultAsync(r => r.Id == model.RecipeId);
            var currentFridge = await _db.Fridges.Include(f => f.Ingredients).Include(f => f.Users).FirstOrDefaultAsync(r => r.Id == model.FridgeId);

            if (currentFridge.Users.Where(u => u.UserId == appUser.Id && u.InvitationAccepted == true) != null)
            {
                if (currentRecipe != null && currentFridge != null)
                {
                    if (appUser.Id != currentRecipe.UserId)
                        return Unauthorized();

                    var enoughIngredients = new List<RecipeIngredientsTextViewModel>();
                    var notEnoughIngredients = new List<RecipeIngredientsTextViewModel>();
                    var currentFridgeIngredient = new FridgeIngredient();
                    foreach (var ri in currentRecipe.Ingredients)
                    {
                        currentFridgeIngredient = currentFridge.Ingredients.Find(i => i.IngredientId == ri.IngredientId);
                        if (currentFridgeIngredient != null)
                        {
                            if (currentFridgeIngredient.Quantity >= ri.Quantity)
                                enoughIngredients.Add(new RecipeIngredientsTextViewModel
                                {
                                    Ingredient = ri.Ingredient.Name,
                                    Category = ri.Ingredient.IngredientCategory.Name,
                                    Quantity = ri.Quantity
                                });
                            else
                            {
                                notEnoughIngredients.Add(new RecipeIngredientsTextViewModel
                                {
                                    Ingredient = ri.Ingredient.Name,
                                    Category = ri.Ingredient.IngredientCategory.Name,
                                    Quantity = ri.Quantity - currentFridgeIngredient.Quantity
                                });
                            }
                        }
                        else
                            notEnoughIngredients.Add(new RecipeIngredientsTextViewModel
                            {
                                Ingredient = ri.Ingredient.Name,
                                Category = ri.Ingredient.IngredientCategory.Name,
                                Quantity = ri.Quantity
                            });
                    }
                    var sufficientIngredients = new SufficientIngredientsViewModel
                    {
                        RecipeId = currentRecipe.Id,
                        FridgeId = currentFridge.Id,
                        EnoughIngredients = enoughIngredients.OrderByDescending(i => i.Quantity).ToList(),
                        NotEnoughIngredients = notEnoughIngredients.OrderByDescending(i => i.Quantity).ToList(),
                        CanUse = notEnoughIngredients.Count() == 0
                    };

                    return Ok(sufficientIngredients);
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpPut("useRecipe")]
        [Authorize]
        public async Task<IActionResult> UseRecipe(UseRecipeViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentRecipe = await _db.Recipes.Include(r => r.Ingredients).ThenInclude(i => i.Ingredient).ThenInclude(i => i.IngredientCategory).FirstOrDefaultAsync(r => r.Id == model.RecipeId);
            var currentFridge = await _db.Fridges.Include(f => f.Ingredients).Include(f => f.Users).FirstOrDefaultAsync(r => r.Id == model.FridgeId);

            if (currentFridge.Users.Where(u => u.UserId == appUser.Id && u.InvitationAccepted == true) != null)
            {
                if (currentRecipe != null && currentFridge != null)
                {
                    if (appUser.Id != currentRecipe.UserId)
                        return Unauthorized();

                    foreach (var fi in currentFridge.Ingredients)
                    {
                        var recipeIngredient = currentRecipe.Ingredients.Where(i => i.IngredientId == fi.IngredientId);
                        if(recipeIngredient.Count() > 0 ) { 
                        fi.Quantity = fi.Quantity - recipeIngredient.Single().Quantity;
                        if (fi.Quantity > 0)
                            _db.Update(fi);
                        else _db.Remove(fi);
                        }
                    }

                    await _db.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            return Unauthorized();
        }
    }
}
