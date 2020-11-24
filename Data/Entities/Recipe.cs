using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int CookingTime { get; set; }
        [ForeignKey("AppUser")]
        public int UserId { get; set; }

        public AppUser AppUser { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }
    }
}
