using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class DataAccess : IDataAccess
    {
        public async Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    var rows = await connection.QueryAsync<T>(sql, parameters);

                    return rows.ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        public async Task<int> SaveData<T>(string sql, T parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                return await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
