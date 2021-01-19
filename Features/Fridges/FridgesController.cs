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

namespace Fridge_BackEnd.Features.Fridges
{
    [Route("api/[controller]")]
    [ApiController]
    public class FridgesController : ControllerBase
    {
        private readonly FridgeContext _db;

        public FridgesController(FridgeContext db)
        {
            _db = db;
        }

        [HttpGet("getUserFridges")]
        [Authorize]
        public async Task<IActionResult> GetUserFridges()
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var userFridges = await _db.Fridges.Where(f => f.Users.Any(u => u.UserId == appUser.Id)).Select(i => new FridgesListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                DateCreated = i.DateCreated,
                DateUpdated = i.DateUpdated,
                IsOwner = i.Users.Where(u => u.UserId == appUser.Id).Select(u => u.IsOwner).Single(),
                InvitationAccepted = i.Users.Where(u => u.UserId == appUser.Id).Select(u => u.InvitationAccepted).Single(),
                InvitationPending = i.Users.Where(u => u.UserId == appUser.Id).Select(u => u.InvitationPending).Single(),
                InvitedBy = i.Users.Where(u => u.IsOwner).Select(u => u.AppUser.FirstName).Single(),
                Ingredients = i.Ingredients.OrderBy(i => i.Quantity).Select(ing => new FridgeIngredients.FridgeIngredientsListViewModel
                {
                    IngredientId = ing.IngredientId,
                    CategoryId = ing.Ingredient.CategoryId,
                    Quantity = ing.Quantity
                }).ToList()
            }).ToListAsync();

            return Ok(userFridges);
        }

        [HttpPost("createFridge")]
        [Authorize]
        public async Task<IActionResult> CreateFridge([FromBody] CreateFridgeViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var newFridge = new Fridge
            {
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
            };

            _db.Fridges.Add(newFridge);
            await _db.SaveChangesAsync();

            var newFridgeUser = new FridgeUser
            {
                UserId = appUser.Id,
                FridgeId = newFridge.Id,
                IsOwner = true,
                InvitationPending = false,
                InvitationAccepted = true
            };

            _db.FridgeUsers.Add(newFridgeUser);

            if (model.Ingredients.Count > 0)
            {
                var fridgeIngredients = new List<FridgeIngredient>();
                model.Ingredients.ForEach(i => fridgeIngredients.Add(new FridgeIngredient
                {
                    FridgeId = newFridge.Id,
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity
                }));

                _db.FridgeIngredients.AddRange(fridgeIngredients);
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("editFridge")]
        [Authorize]
        public async Task<IActionResult> EditFridge([FromBody] EditFridgeViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentFridge = await _db.Fridges.Include(f=> f.Users).FirstOrDefaultAsync(r => r.Id == model.FridgeId);
            if (currentFridge != null)
            {
                if (currentFridge.Users.Where(u => u.UserId == appUser.Id && u.InvitationAccepted == true).FirstOrDefault() == null)
                    return Unauthorized();

                currentFridge.Name = model.Name;
                currentFridge.Description = model.Description;

                _db.Fridges.Update(currentFridge);
                await _db.SaveChangesAsync();

                var currentFridgeIngredients = await _db.FridgeIngredients.Where(ri => ri.FridgeId == model.FridgeId).ToListAsync();
                _db.FridgeIngredients.RemoveRange(currentFridgeIngredients);

                var fridgeIngredients = new List<FridgeIngredient>();
                model.Ingredients.ForEach(i => fridgeIngredients.Add(new FridgeIngredient
                {
                    FridgeId = model.FridgeId,
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity
                }));

                _db.FridgeIngredients.AddRange(fridgeIngredients);
                await _db.SaveChangesAsync();

                return Ok();
            }
            else
                return NotFound();
        }

        [HttpPut("addProducts")]
        [Authorize]
        public async Task<IActionResult> AddProducts(AddProductsViewModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentFridge = await _db.Fridges.Include(f => f.Users).FirstOrDefaultAsync(f => f.Id == model.FridgeId);
            if (currentFridge != null)
            {
                if (currentFridge.Users.Where(u => u.UserId == appUser.Id && u.InvitationAccepted == true).FirstOrDefault() == null)
                    return Unauthorized();
                else
                {
                    var currentFridgeIngredients = await _db.FridgeIngredients.Where(f => f.FridgeId == currentFridge.Id).ToListAsync();
                    var newFridgeIngredients = new List<FridgeIngredient>();
                    foreach( var i in model.Ingredients)
                    {
                        if (currentFridgeIngredients.Where(fi => fi.IngredientId == i.IngredientId).FirstOrDefault() != null)
                            currentFridgeIngredients.Where(fi => fi.IngredientId == i.IngredientId).FirstOrDefault().Quantity += i.Quantity;
                        else newFridgeIngredients.Add(new FridgeIngredient
                        {
                            FridgeId = model.FridgeId,
                            IngredientId = i.IngredientId,
                            Quantity = i.Quantity
                        });
                    }
                     _db.FridgeIngredients.UpdateRange(currentFridgeIngredients);
                     _db.FridgeIngredients.AddRange(newFridgeIngredients);
                    await _db.SaveChangesAsync();
                    return Ok();
                }
            }
            return NotFound();
        }

        [HttpDelete("deleteFridge/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFridge(int id)
            {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            var currentFridge = await _db.Fridges.FirstOrDefaultAsync(f => f.Id == id);
            if (currentFridge != null)
            {
                if (_db.FridgeUsers.Where(fu => fu.AppUser.Id == appUser.Id).FirstOrDefault() == null || _db.FridgeUsers.Where(fu => fu.AppUser.Id == appUser.Id).FirstOrDefault().IsOwner == false)
                    return Unauthorized();

                var currentFridgeIngredients = await _db.FridgeIngredients.Where(ri => ri.FridgeId == id).ToListAsync();
                _db.FridgeIngredients.RemoveRange(currentFridgeIngredients);

                _db.Fridges.Remove(currentFridge);
                await _db.SaveChangesAsync();

                return Ok();
            }
            else
                return NotFound();
        }

    }
}
