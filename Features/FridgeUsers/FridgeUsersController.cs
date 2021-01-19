using System.Linq;
using System.Threading.Tasks;
using Fridge_BackEnd.Data;
using Fridge_BackEnd.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fridge_BackEnd.Features.FridgeUsers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FridgeUsersController : ControllerBase
    {
        private readonly FridgeContext _db;

        public FridgeUsersController(FridgeContext db)
        {
            _db = db;
        }


        [HttpPost("inviteUserToFridge")]
        [Authorize]
        public async Task<IActionResult> InviteUserToFridge([FromBody] InviteUserToFridgeModel model)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
            if (appUser.Email.ToLower() != model.Email)
            {
                var fridge = await _db.Fridges.Include(f => f.Users).FirstOrDefaultAsync(f => f.Id == model.FridgeId);
                if (fridge != null && fridge.Users.Where(f => f.UserId == appUser.Id).FirstOrDefault().IsOwner)
                {
                    var invitedUser = _db.Users.Where(u => u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault();
                    if (invitedUser != null)
                    {
                        var fridgeUserEntries = _db.FridgeUsers.Where(f => f.FridgeId == fridge.Id &&
                        f.UserId == invitedUser.Id);

                        if (fridgeUserEntries.Where(f => f.InvitationAccepted == true || f.InvitationPending == true).Count() > 0)
                            return Ok();
                        else
                        {
                            _db.FridgeUsers.Add(new FridgeUser
                            {
                                FridgeId = fridge.Id,
                                UserId = invitedUser.Id,
                                IsOwner = false,
                                InvitationAccepted = false,
                                InvitationPending = true
                            });
                            await _db.SaveChangesAsync();
                            return Ok();
                        }
                    }
                    else
                        return Ok();
                }
                return Ok();
            }
            else return Conflict(new { message = "Inviting yourself is not possible" });
        }

        [HttpPost("acceptInvitation/{id}")]
        [Authorize]
        public async Task<IActionResult> AcceptInvitation(int id)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            var fridgeInvitation = _db.FridgeUsers.Where(f => f.FridgeId == id && f.UserId == appUser.Id && f.InvitationPending).Single();
            if (fridgeInvitation != null)
            {
                fridgeInvitation.InvitationPending = false;
                fridgeInvitation.InvitationAccepted = true;
                _db.FridgeUsers.Update(fridgeInvitation);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else return NotFound(new { message = "The invitation was not found" });
        }

        [HttpPost("declineInvitation/{id}")]
        [Authorize]
        public async Task<IActionResult> DeclineInvitation(int id)
        {
            var appUser = await _db.Users.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            var fridgeInvitation = _db.FridgeUsers.Where(f => f.FridgeId == id && f.UserId == appUser.Id && f.InvitationPending).Single();
            if (fridgeInvitation != null)
            {
                fridgeInvitation.InvitationPending = false;
                fridgeInvitation.InvitationAccepted = false;
                _db.FridgeUsers.Update(fridgeInvitation);
                await _db.SaveChangesAsync();
                return Ok();
            }
            else return NotFound(new { message = "The invitation was not found" });
        }
    }
}
