using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Domain.Models
{
    public class BetModel
    {
        public int? Number { get; set; }
        public string Color { get; set; }
        public decimal Amount { get; set; }
    }
}
