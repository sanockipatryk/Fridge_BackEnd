using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.Ingredients
{
    public class IngredientListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
    }
}
