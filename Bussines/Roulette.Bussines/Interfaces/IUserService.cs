using Roulette.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Bussines.Interfaces
{
    public interface IUserService
    {
        public Task CreateRoulette(RouletteModel roulettemodel);
        public Task<bool> ChangeState(string rouletteId, RouletteState newState);
        public Task CreateUser(UserModel userModel);
        public Task<bool> CreateBet(string idRoulette, string userId, BetRequestModel bet);
        public Task<bool> IsRouletteOpen(string rouletteId);
        public Task<List<BetModel>> BetsPlacedByRoulette(string rouletteId);
        public Task<List<RouletteModel>> GetAllRoulettes();
        public Task<RouletteModel> GetRouletteById(string rouletteId);
        public Task<UserModel> GetUserById(string userId);
    }
}
