using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class LoginQuery
    {
        public LoginViewModel CheckUserLogin(string? username, string? password)
        {
            LoginViewModel dataUser = new LoginViewModel();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string querySql = "SELECT * FROM [Users] WHERE [Username] = @username AND [Password] = @password";
                SqlCommand cmd = new SqlCommand(querySql, conn);
                cmd.Parameters.AddWithValue("@username", username ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@password", password ?? (object)DBNull.Value);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataUser.Id = reader["Id"].ToString();
                        dataUser.UserName = reader["Username"].ToString();
                        dataUser.RoleId = reader["RoleId"].ToString();
                        dataUser.Email = reader["Email"].ToString();
                        dataUser.Phone = reader["Phone"].ToString();
                        dataUser.Extracode = reader["ExtraCode"].ToString();
                    }
                    conn.Close();
                }
            }
            return dataUser;
        }
    }
}
