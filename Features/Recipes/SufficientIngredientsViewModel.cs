using Fridge_BackEnd.Data.Entities;
using Fridge_BackEnd.Features.RecipeIngredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.Recipes
{
    public class SufficientIngredientsViewModel
    {
        public int RecipeId { get; set; }
        public int FridgeId { get; set; }
        public List<RecipeIngredientsTextViewModel> EnoughIngredients { get; set; }
        public List<RecipeIngredientsTextViewModel> NotEnoughIngredients { get; set; }
        public bool CanUse { get; set; }
    }
}
