using Npgsql;
using Roulette.Domain.Models;
using Roulette.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<bool> CreateBet(string idRoulette, string userId, BetRequestModel bet)
        {
            var connection = await _client.GetConnection();
            var sql = @"INSERT INTO ""Apuesta"" (""IdRoulette"", ""UserId"", ""Number"", ""Color"", ""Amount"", ""IdBet"") VALUES (@IdRoulette, @UserId, @Number, @Color, @Amount, @IdBet)";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("IdRoulette", idRoulette);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("Number", (object?)bet.Number ?? DBNull.Value);
                cmd.Parameters.AddWithValue("Color", (object?)bet.Color ?? DBNull.Value);
                cmd.Parameters.AddWithValue("Amount", bet.Amount);
                cmd.Parameters.AddWithValue("IdBet", Guid.NewGuid().ToString());
                var rowsAffected = await cmd.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> IsRouletteOpen (string rouletteId)
        {
            var connection = await _client.GetConnection();
            var sql = @"SELECT ""State"" FROM ""Ruleta"" WHERE ""IdRoulette"" = @IdRoulette";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("Idroulette", rouletteId);

                var result = await cmd.ExecuteScalarAsync();
                if (result == null) 
                {
                    return false;
                }
                return result.ToString() == RouletteState.Open.ToString();
            }
        }

        public async Task <List<BetModel>> BetsPlacedByRoulette(string rouletteId) 
        { 
            var bets = new List<BetModel>();
            var connection = await _client.GetConnection();
            string sql = @"SELECT ""IdRoulette"", ""UserId"", ""Number"", ""Color"", ""Amount"", ""IdBet"" FROM ""Apuesta"" WHERE ""IdRoulette"" = @IdRoulette";

            using ( var cmd = new NpgsqlCommand(sql,connection))
            
            {
                cmd.Parameters.AddWithValue("@IdRoulette", rouletteId);
                using ( var reader = await cmd.ExecuteReaderAsync() )
                {
                    while (await reader.ReadAsync())
                    {
                        bets.Add(new BetModel
                        {
                            IdRoulette = reader.GetString(0),
                            UserId = reader.GetString(1),
                            Number = reader.IsDBNull(2) ? null : reader.GetInt16(2),
                            Color = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Amount = reader.GetDecimal(4),
                            IdBet = reader.GetString(5),
                        });
                    }
                }
            }
            return bets;

        }

    }
}
