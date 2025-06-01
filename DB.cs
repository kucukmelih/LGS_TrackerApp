using System.Data;
using System;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace LGS_Tracker
{
    // Static class to handle MySQL database operations
    public class DB
    {
        // Read the connection string from App.config (key: DefaultConnection)
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        // Returns a new MySqlConnection object
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        // Executes a SELECT query and returns the result as a DataTable
        public static DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table); // Fill table with query results
                        return table;
                    }
                }
            }
        }

        // Executes INSERT, UPDATE, or DELETE command, returns number of affected rows
        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery(); // Returns affected row count
                }
            }
        }

        // Executes a scalar query (returns a single value), e.g. for getting an ID or COUNT
        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar(); // Returns single value result
                }
            }
        }
    }
}
