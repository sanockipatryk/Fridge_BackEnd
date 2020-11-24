using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class FridgeIngredient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        [ForeignKey("Ingredient")]
        public int IngredientId { get; set; }

        [Required]
        public Fridge Fridge { get; set; }
        [Required]
        public Ingredient Ingredient { get; set; }
    }
}
