using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Domain.Models
{
    public class BetModel
    {
        public string IdRoulette { get; set; }
        public string UserId { get; set; }
        public int? Number { get; set; }
        public string Color { get; set; }
        public decimal Amount { get; set; }

        [Key]
        public string IdBet { get; set; } 
    }
}
