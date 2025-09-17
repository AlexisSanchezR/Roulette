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

        public async Task CreateRoulette(UserModel userModel)
        {
            try
            {
                await _dbRepository.CreateRoulette(userModel);
            }
            catch (Exception)
            {
                Log.Error($"Error: {JsonConvert.SerializeObject(userModel)}");
                throw;
            }
        }
    }
}
