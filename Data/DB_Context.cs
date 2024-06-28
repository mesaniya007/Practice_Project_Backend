using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace user_crud_api.Data
{
    public class DB_Context
    {
        public void addUser(User user)
        {
            using (NpgsqlConnection conn = DB_Connection.OpenConnection())
            {
                string query = "CALL addUser(@name,@email,@dob,@phone,@password)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@dob", user.Birth_date);
                    cmd.Parameters.AddWithValue("@phone", user.Phone);
                    var hashPassword = getHash(user.Password);
                    cmd.Parameters.AddWithValue("@password", hashPassword);
                    cmd.ExecuteScalar();
                }
            }
        }
        public Login getUserByEmail(string userEmail)
        {
            using (NpgsqlConnection conn = DB_Connection.OpenConnection())
            {
                Login login = new Login();
                string query = "SELECT * FROM getUserByEmail(@email)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", userEmail);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            login.Email = reader["email"].ToString();
                            login.Password = reader["password"].ToString();
                        }
                    }
                    return login;
                }
            }
        }
        private static string getHash(string pass)
        {
            return BCrypt.Net.BCrypt.HashPassword(pass);
        }





    }
}





