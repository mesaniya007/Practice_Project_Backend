using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using user_crud_api.Helpers;

namespace user_crud_api.Data
{
    public class DB_Context
    {
        PasswordHasher passwordHasher = new PasswordHasher();
        public void addUser(User user)
        {
            using (NpgsqlConnection conn = DB_Connection.OpenConnection())
            {
                string query = "CALL addUser(@name,@email,@dob,@phone,@password,@approved)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@dob", user.Birth_date);
                    cmd.Parameters.AddWithValue("@phone", user.Phone);
                    var hashPassword = passwordHasher.HashPassword(user.Password);
                    cmd.Parameters.AddWithValue("@password", hashPassword);
                    cmd.Parameters.AddWithValue("@approved", user.Approved);
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
        public List<User> getAllUser()
        {
            using (NpgsqlConnection conn = DB_Connection.OpenConnection())
            {
                var users = new List<User>();
                string query = "SELECT * FROM getAllUsers()";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(reader.GetOrdinal("user_name")) == "Admin"){ }
                            else
                            {
                                users.Add(new User
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                                    Name = reader.GetString(reader.GetOrdinal("user_name")),
                                    Email = reader.GetString(reader.GetOrdinal("user_email")),
                                    Birth_date = reader.GetDateTime(reader.GetOrdinal("user_birth_date")),
                                    Phone = reader.GetString(reader.GetOrdinal("user_phone")),
                                    Approved = reader.GetBoolean(reader.GetOrdinal("user_approved")),
                                });
                            }
                            
                        }
                    }
                    return users;
                }
            }
        }

        public void approveUser(int id)
        {
            using (NpgsqlConnection conn = DB_Connection.OpenConnection())
            {
                string query = "UPDATE users SET approved = @approved WHERE id = @user_id;";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user_id", id);
                    cmd.Parameters.AddWithValue("@approved", true);
                    cmd.ExecuteReader();
                }
            }
        }

    }
}





