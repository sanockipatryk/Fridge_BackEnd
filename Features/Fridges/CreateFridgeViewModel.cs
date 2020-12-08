using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fridge_BackEnd.Features.Fridges
{
    public class CreateFridgeViewModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        
    }
}
