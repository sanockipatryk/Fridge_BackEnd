using Fridge_BackEnd.Features.FridgeIngredients;
using Fridge_BackEnd.Features.RecipeIngredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.Fridges
{
    public class EditFridgeViewModel
    {
        public int FridgeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CreateFridgeIngredientsViewModel> Ingredients { get; set; }
    }
}
