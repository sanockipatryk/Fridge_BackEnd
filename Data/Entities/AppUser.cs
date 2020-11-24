using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fridge_BackEnd.Data.Entities
{
    public class AppUser: IdentityUser<int>
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<Recipe> Recipes { get; set; }
        public List<FridgeUser> Fridges { get; set; }

    }
}
