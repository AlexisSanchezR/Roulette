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
        public async Task<bool> CreateBet(string idRoulette, string userId, BetRequestModel bet)
        {
            //Validaciones
            if (bet.Amount <= 0 || bet.Amount >10000)
            {
                throw new Exception("Bet amount must be between 1 and 10000");
            }
            if (bet.Number.HasValue && (bet.Number < 0 || bet.Number > 36))
            {
                throw new Exception("Number bet must be between 0 and 36");
            }
            if (!string.IsNullOrEmpty(bet.Color) &&
                    bet.Color.ToLower() != "red" && 
                    bet.Color.ToLower() != "black")
            {
                throw new Exception("Color bet must be 'red' or 'black'");
            }
           
            //validación de la ruleta
            //DEbo verificar tambien que la ruleta exista ((Metodo Get))
            var rouletteOpen = await _dbRepository.IsRouletteOpen(idRoulette);
            if (!rouletteOpen)
            {
                throw new Exception("Roulette is not Open");
            }

            //crear apuesta 
            var success = await _dbRepository.CreateBet(idRoulette, userId, bet);
            if (!success)
            {
                throw new Exception("Error placing Bet");
            }
            return true;
            
        }

        public async Task<bool> IsRouletteOpen(string rouletteId)
        {
            return await _dbRepository.IsRouletteOpen(rouletteId);
        }

        public async Task<List<BetModel>> BetsPlacedByRoulette(string rouletteId)
        {
            return await _dbRepository.BetsPlacedByRoulette(rouletteId);
        }
    }
}
