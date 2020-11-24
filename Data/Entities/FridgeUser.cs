using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Data.Entities
{
    public class FridgeUser
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public int UserId { get; set; }
        [ForeignKey("Fridge")]
        public int FridgeId { get; set; }
        [Required]
        public bool IsOwner { get; set; }

        [Required]
        public AppUser AppUser { get; set; }
        [Required]
        public Fridge Fridge { get; set; }
    }
}
