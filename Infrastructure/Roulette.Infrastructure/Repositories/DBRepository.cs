using Npgsql;
using Roulette.Domain.Models;
using Roulette.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task CreateRoulette(UserModel userModel)
        {
            var connection = await _client.GetConnection();
            var sql = $"INSERT INTO \"Ruleta\" (\"IdRoulette\", \"Roulettename\") VALUES (@IdRoulette,@Roulettename)";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("IdRoulette", userModel.IdRoulette);
                cmd.Parameters.AddWithValue("Roulettename", userModel.Roulettename);
                await cmd.ExecuteNonQueryAsync();
            }

        }
    }
}
