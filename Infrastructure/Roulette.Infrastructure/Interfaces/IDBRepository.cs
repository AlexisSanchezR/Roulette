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
        public Task CreateRoulette(UserModel userModel);
    }
}
