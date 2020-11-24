using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class Fridge
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public List<FridgeUser> Users { get; set; }
        public List<FridgeIngredient> Ingredients { get; set; }
    }
}
