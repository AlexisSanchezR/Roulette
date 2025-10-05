using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Roulette.Domain.Models
{
    public class RouletteModel
    {
        [Key]
        public string IdRoulette { get; set; } = Guid.NewGuid().ToString();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RouletteState State { get; set; } = RouletteState.Close;
    }

    public enum RouletteState
    {
        Close = 0, Open = 1
    }
}
