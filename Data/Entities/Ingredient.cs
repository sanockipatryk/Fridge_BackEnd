using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("IngredientCategory")]
        public int CategoryId { get; set; }

        [Required]
        public IngredientCategory IngredientCategory { get; set; }
        public List<FridgeIngredient> Fridges { get; set; }
        public List<RecipeIngredient> Recipes { get; set; }
    }
}
