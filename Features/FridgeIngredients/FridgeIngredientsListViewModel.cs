using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.FridgeIngredients
{
    public class FridgeIngredientsListViewModel
    {
        public int IngredientId { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
