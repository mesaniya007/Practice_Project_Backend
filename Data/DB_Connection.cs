using Npgsql;

namespace user_crud_api.Data
{
    public class DB_Connection
    {
        private static string _cs = "Host = localhost; Database=UserDB;Username=postgres;Password=Asif786";

        public static NpgsqlConnection OpenConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(_cs);
            conn.Open();
            return conn;
        }

        public static void CloseConnection(NpgsqlConnection connection)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
