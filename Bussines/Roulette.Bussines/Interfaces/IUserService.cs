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
        public Task CreateRoulette(UserModel userModel);
    }
}
