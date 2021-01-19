using System.Collections.Generic;
using Fridge_BackEnd.Features.FridgeIngredients;

namespace Fridge_BackEnd.Features.Fridges
{
    public class AddProductsViewModel
    {
        public int FridgeId { get; set; }
        public List<CreateFridgeIngredientsViewModel> Ingredients { get; set; }
    }
}