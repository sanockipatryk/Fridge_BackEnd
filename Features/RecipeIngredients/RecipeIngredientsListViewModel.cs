using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.RecipeIngredients
{
    public class RecipeIngredientsListViewModel
    {
        public int IngredientId { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
