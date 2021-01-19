using System.ComponentModel.DataAnnotations;

namespace Fridge_BackEnd.Features.FridgeUsers
{
    public class InviteUserToFridgeModel
    {
        [Required]
        public int FridgeId { get; set; }
        [Required]
        public string Email { get; set; }
    }
}