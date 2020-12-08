using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fridge_BackEnd.Features.Fridges
{
    public class FridgesListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsOwner { get; set; }
    }
}
