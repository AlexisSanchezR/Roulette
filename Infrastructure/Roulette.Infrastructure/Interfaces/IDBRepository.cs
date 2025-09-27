using Roulette.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Interfaces
{
    public interface IDBRepository
    {
        public Task CreateRoulette(RouletteModel rouletteModel);
        public Task<bool> ChangeState(string rouletteId, RouletteState newState);
        public Task CreateUser(UserModel userModel);
        public Task<bool> CreateBet(string idRoulette, string userId, BetRequestModel bet);
        public Task<bool> IsRouletteOpen(string rouletteId);
        public Task<List<BetModel>> BetsPlacedByRoulette(string rouletteId);
        public Task<List<RouletteModel>> GetAllRoulettes();
    }
}
