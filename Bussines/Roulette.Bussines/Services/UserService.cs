using Newtonsoft.Json;
using Roulette.Bussines.Interfaces;
using Roulette.Domain.Models;
using Roulette.Infrastructure.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Bussines.Services
{
    public class UserService : IUserService
    {
        private readonly IDBRepository _dbRepository;
        public UserService( IDBRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task CreateRoulette(RouletteModel roulettemodel)
        {
            try
            {
                await _dbRepository.CreateRoulette(roulettemodel);
            }
            catch (Exception)
            {
                Log.Error($"Error: {JsonConvert.SerializeObject(roulettemodel)}");
                throw;
            }
        }
        public async Task<bool> ChangeState(string rouletteId, RouletteState newState)
        {
            return await _dbRepository.ChangeState(rouletteId, newState);
        }

        public async Task CreateUser(UserModel userModel)
        {
            try
            {
                await _dbRepository.CreateUser(userModel);
            }
            catch (Exception)
            {
                Log.Error($"Error: {JsonConvert.SerializeObject(userModel)}");
                throw;
            }
        }
        public async Task<bool> CreateBet(string idRoulette, string userId, BetModel bet)
        {
           return await _dbRepository.CreateBet(idRoulette, userId, bet);
        }
    }
}
