using Npgsql;
using Roulette.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roulette.Infrastructure.Client
{
    public class DBClient : IDBClient 
    {
        private readonly string _dbConnectionString;
        public DBClient (string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public async Task<NpgsqlConnection> GetConnection()
        {
            var connection = new NpgsqlConnection (_dbConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
