using Microsoft.EntityFrameworkCore;
using Roulette.Domain.Models;
using Roulette.Infrastructure.Data;
using Roulette.Infrastructure.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Repositories
{
    public class DBRepositoryEF : IDBRepositoryEF
    {
        private readonly AppDbContext _context;
        public DBRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateRoulette (RouletteModel rouletteModel)
        {
            await _context.Roulette.AddAsync (rouletteModel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ChangeState (string rouletteId, RouletteState newState)
        {
            var roulette = await _context.Roulette.FirstOrDefaultAsync(r => r.IdRoulette == rouletteId);
            if (roulette == null)
            {
                return false;
            }
            roulette.State = newState;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task CreateUser(UserModel userModel)
        {
            await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CreateBet (string idRoulette, string userId, BetModel bet)
        {
            try
            {
                var newBet = new BetModel
                {
                    IdBet = Guid.NewGuid().ToString(),
                    IdRoulette = idRoulette,
                    UserId = userId,
                    Number = bet.Number,
                    Color = bet.Color,
                    Amount = bet.Amount
                };
                _context.Bet.Add(newBet);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("error creating bet");
                return false;
            }
        }
        public async Task<bool> IsRouletteOpen (string rouletteId)
        {
            var roulette = await _context.Roulette.AsNoTracking().FirstOrDefaultAsync(r => r.IdRoulette == rouletteId);
            if (roulette == null)
            {
                return false;
            }
            //Devuelve true, solo si roulette.state = Open.
            return roulette.State == RouletteState.Open;
        }
        public async Task<List<BetModel>> BetsPlacedByRoulette (string rouletteId)
        {
            var bets = await _context.Bet.Where(b => b.IdRoulette == rouletteId).ToListAsync();

            return bets;
        }

        public async Task<List<RouletteModel>> GetAllRoulettes()
        {
            var roulettes = await _context.Roulette.ToListAsync();
            return roulettes;
        }
        public async Task<RouletteModel> GetRouletteById (string rouletteId)
        {
            var roulette = await _context.Roulette.FirstOrDefaultAsync(r => r.IdRoulette == rouletteId);
            if(roulette == null)
            {
                Log.Error("An error occurred while finding the roulette by id.");
                return null;
            }
            return roulette;
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == userId);
            if(user == null)
            {
                Log.Error("An error occurred while finding the user by id.");
                return null;
            }
            return user;
        }

        public async Task<bool> UpdateUserCredit (UserModel user)
        {
            var userUpdate = await _context.Users.FirstOrDefaultAsync(u => u.IdUser == user.IdUser);
            if (userUpdate == null)
            {
                Log.Error("User not found");
                return false;
            }
            userUpdate.Credit = user.Credit;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
