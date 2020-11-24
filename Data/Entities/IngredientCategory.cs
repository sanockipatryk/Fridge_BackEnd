using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class IngredientCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
