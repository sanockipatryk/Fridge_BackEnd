using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fridge_BackEnd.Features.FridgeIngredients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fridge_BackEnd.Features.Fridges
{
    public class CreateFridgeViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CreateFridgeIngredientsViewModel> Ingredients { get; set; }

    }
}
