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
            var userFridges = await _db.Fridges.Where(f => f.Users.Any(u => u.UserId == appUser.Id)).Select( i => new FridgesListViewModel
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                DateCreated = i.DateCreated,
                DateUpdated = i.DateUpdated,
                IsOwner = i.Users.Where(u => u.UserId == appUser.Id).Select(u => u.IsOwner).Single()
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
                IsOwner = true
            };

            _db.FridgeUsers.Add(newFridgeUser);
            await _db.SaveChangesAsync();

            return Ok();
        }

    }
}
