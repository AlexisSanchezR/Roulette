using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Domain.Models
{
    public class UserModel
    {
        [Key]
        public string IdUser { get; set; } =Guid.NewGuid().ToString();
        public decimal Credit { get; set; }
    }
}
