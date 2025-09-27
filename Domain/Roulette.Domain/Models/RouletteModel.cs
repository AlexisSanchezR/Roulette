using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Roulette.Domain.Models
{
    public class RouletteModel
    {
        public string IdRoulette { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RouletteState State { get; set; } = RouletteState.Close;
    }

    public enum RouletteState
    {
        Close = 0, Open = 1
    }
}
