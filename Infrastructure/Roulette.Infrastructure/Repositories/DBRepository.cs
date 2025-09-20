using Npgsql;
using Roulette.Domain.Models;
using Roulette.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Repositories
{
    public class DBRepository : IDBRepository
    {
        private readonly IDBClient _client;
        public DBRepository(IDBClient client)
        {
            _client = client;
        }

        public async Task CreateRoulette(RouletteModel rouletteModel)
        {
            var connection = await _client.GetConnection();
            var sql = $"INSERT INTO \"Ruleta\" (\"IdRoulette\", \"State\") VALUES (@IdRoulette, @State)";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("IdRoulette", rouletteModel.IdRoulette);
                cmd.Parameters.AddWithValue("State", rouletteModel.State.ToString());
                await cmd.ExecuteNonQueryAsync();
            }

        }
        public async Task<bool> ChangeState (string rouletteId, RouletteState newState)
        {
            var connection = await _client.GetConnection();
            string sql = @"UPDATE ""Ruleta"" SET ""State"" = @State WHERE ""IdRoulette"" = @IdRoulette";
            using (var cmd = new NpgsqlCommand(sql,connection))
            {
                cmd.Parameters.AddWithValue("State", newState.ToString());
                cmd.Parameters.AddWithValue("IdRoulette", rouletteId);
                var rowsAffected =  await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task CreateUser (UserModel userModel)
        {
            var connection = await _client.GetConnection();
            var sql = $"INSERT INTO \"User\" (\"IdUser\", \"Credit\") VALUES (@IdUser,@Credit)";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("IdUser", userModel.IdUser);
                cmd.Parameters.AddWithValue("Credit", userModel.Credit);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> CreateBet(string idRoulette, string userId, BetModel bet)
        {
            var connection = await _client.GetConnection();
            var sql = @"INSERT INTO ""Apuesta"" (""IdRoulette"", ""UserId"", ""Number"", ""Color"", ""Amount"") VALUES (@IdRoulette, @UserId, @Number, @Color, @Amount)";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("IdRoulette", idRoulette);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("Number", (object?)bet.Number ?? DBNull.Value);
                cmd.Parameters.AddWithValue("Color", (object?)bet.Color ?? DBNull.Value);
                cmd.Parameters.AddWithValue("Amount", bet.Amount);
                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
