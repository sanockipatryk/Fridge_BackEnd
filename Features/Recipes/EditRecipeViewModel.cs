using Fridge_BackEnd.Features.RecipeIngredients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.Recipes
{
    public class EditRecipeViewModel
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CookingTime { get; set; }
        public List<CreateRecipeIngredientsViewModel> Ingredients { get; set; }
    }
}
