using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.RecipeIngredients
{
    public class RecipeIngredientsTextViewModel
    {
        public string Ingredient { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}
