using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class RecipeIngredient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }

        [Required]
        public Recipe Recipe { get; set; }
        [Required]
        public Ingredient Ingredient { get; set; }
    }
}
