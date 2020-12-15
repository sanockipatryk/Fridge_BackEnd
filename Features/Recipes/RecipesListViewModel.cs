using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fridge_BackEnd.Features.RecipeIngredients;

namespace Fridge_BackEnd.Features.Recipes
{
    public class RecipesListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int CookingTime { get; set; }
        public List<RecipeIngredientsListViewModel> Ingredients { get; set; }
    }
}
