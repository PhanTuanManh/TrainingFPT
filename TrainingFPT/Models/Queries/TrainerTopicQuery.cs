using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;
using TrainingFPT.Migrations;
using Microsoft.AspNetCore.Session;

namespace TrainingFPT.Models.Queries
{
    public class TrainerTopicQuery
    {
        public List<UserDetail> GetTrainer()
        {
            List<UserDetail> trainer = new List<UserDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Users] WHERE RoleId = 3;";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Kiểm tra xem có dữ liệu để đọc hay không
                    {
                        UserDetail UserDetail = new UserDetail();
                        UserDetail.UserName = reader["Username"].ToString();
                        UserDetail.Id = Convert.ToInt32(reader["Id"]);
                        trainer.Add(UserDetail);

                    }
                }
                conn.Close();
            }
            return trainer;
        }
            public List<TopicDetail> GetTopic()
            {
                List<TopicDetail> topic = new List<TopicDetail>();
                using (SqlConnection conn = Database.GetSqlConnection())
                {
                    string sqlQuery = "SELECT * FROM [Topics];";
                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) // Kiểm tra xem có dữ liệu để đọc hay không
                        {
                            TopicDetail topicDetail = new TopicDetail();
                            topicDetail.Name = reader["NameTopic"].ToString();
                            topicDetail.Id = Convert.ToInt32(reader["Id"]);
                            topic.Add(topicDetail);

                        }
                    }
                    conn.Close();
                }
                return topic;
            }
        public int InsertItemTrainerTopic(
            
            int userId,
            int topicId
        )
        {
            int lastIdInsert = 0; // id cua category vua dc them moi
            string sqlQuery = "INSERT INTO [TrainerTopic]([UserId],[TopicId], [CreatedAt ]) VALUES(@userId, @topicId,  @createdAt) SELECT SCOPE_IDENTITY()";
            // SELECT SCOPE_IDENTITY() : lay ra id vua dc them moi
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@userId", userId );
                cmd.Parameters.AddWithValue("@topicId", topicId );
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastIdInsert = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            // lastIdInsert tra ve lon hon 0 insert thanh cong va nguoc lai
            return lastIdInsert;
        }
        public List<TrainerTopicDetail> GetAllTrainerTopic()
        {
            Dictionary<int, string> Username = new Dictionary<int, string>();
            Dictionary<int, string> TopicName = new Dictionary<int, string>();
            List<TrainerTopicDetail> trainerTopic = new List<TrainerTopicDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {

                string sqlQuery = string.Empty;
               
                
                    sqlQuery = "SELECT * FROM [TrainerTopic]";
                

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                
                conn.Open();
                using (SqlCommand cmdTrainerTopic = new SqlCommand("SELECT Id, Username FROM Users", conn))
                {
                    using (SqlDataReader readerTrainerTopic = cmdTrainerTopic.ExecuteReader())
                    {
                        while (readerTrainerTopic.Read())
                        {
                            Username.Add(Convert.ToInt32(readerTrainerTopic["Id"]), readerTrainerTopic["Username"].ToString());
                        }
                    }
                }
                conn.Close();
                conn.Open();
                using (SqlCommand cmdTrainerTopic = new SqlCommand("SELECT Id, NameTopic FROM Topics", conn))
                {
                    using (SqlDataReader readerTrainerTopic = cmdTrainerTopic.ExecuteReader())
                    {
                        while (readerTrainerTopic.Read())
                        {
                            TopicName.Add(Convert.ToInt32(readerTrainerTopic["Id"]), readerTrainerTopic["NameTopic"].ToString());
                        }
                    }
                }
                conn.Close();
               
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TrainerTopicDetail trainerTopicDetail = new TrainerTopicDetail();
                        trainerTopicDetail.UserId = Convert.ToInt32(reader["UserId"]);
                        trainerTopicDetail.TopicId = Convert.ToInt32(reader["TopicId"]);
                        trainerTopicDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        if (reader["UpdatedAt"] != DBNull.Value)
                        {
                            trainerTopicDetail.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                        }
                        if (Username.ContainsKey(trainerTopicDetail.UserId))
                        {
                            trainerTopicDetail.UserName = Username[trainerTopicDetail.UserId];
                        }
                        if (TopicName.ContainsKey(trainerTopicDetail.TopicId))
                        {
                            trainerTopicDetail.TopicName = TopicName[trainerTopicDetail.TopicId];
                        }
                        trainerTopic.Add(trainerTopicDetail);
                    }
                    conn.Close();
                }
            }
            return trainerTopic;
        }
        public TrainerTopicDetail GetDataTrainerTopicById(int userId = 0, int topicId = 0)
        {

            TrainerTopicDetail trainerTopicDetail = new TrainerTopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT TrainerTopic.UserId, TrainerTopic.TopicId, Users.Username AS UserName, Topics.NameTopic AS TopicName FROM TrainerTopic INNER JOIN Users ON TrainerTopic.UserID = Users.ID INNER JOIN Topics ON TrainerTopic.TopicID = Topics.ID WHERE [UserId] = @userId AND [TopicId] = @topicId; ";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@topicId", topicId);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                       trainerTopicDetail.UserId = Convert.ToInt32(reader["UserId"]);
                       trainerTopicDetail.TopicId = Convert.ToInt32(reader["TopicId"]);
                        trainerTopicDetail.TopicName = reader["TopicName"].ToString();
                        trainerTopicDetail.UserName = reader["UserName"].ToString();
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return trainerTopicDetail;
        }
        public bool UpdateTrainerTopicById(
            int userId,
            int topicId
        )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [TrainerTopic] SET [UserId] = @userId, [TopicId] = @topicId, [UpdatedAt] = @updatedAt WHERE [UserId] = @userId";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.Parameters.AddWithValue("@topicId", topicId);
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
        //public List<TrainerTopicDetail> ViewCourseById()
        //{
        //    List<TrainerTopicDetail> trainerTopic = new List<TrainerTopicDetail>();
        //    TrainerTopicDetail trainerTopicDetail = new TrainerTopicDetail();
        //    using (SqlConnection connection = Database.GetSqlConnection())
        //    {
        //        string sqlQuery = "SELECT TrainerTopic.UserId, TrainerTopic.TopicId, Users.Username AS UserName, Topics.NameTopic AS TopicName FROM TrainerTopic INNER JOIN Users ON TrainerTopic.UserID = Users.ID INNER JOIN Topics ON TrainerTopic.TopicID = Topics.ID WHERE [UserId] = @userId ";
        //        connection.Open();
        //        SqlCommand cmd = new SqlCommand(sqlQuery, connection);
        //        string id = HttpContextAccessor.HttpContext.Session.GetString("SessionUsername");
        //        cmd.Parameters.AddWithValue("@userId", );
               
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                trainerTopicDetail.UserId = Convert.ToInt32(reader["UserId"]);
        //                trainerTopicDetail.TopicId = Convert.ToInt32(reader["TopicId"]);
        //                trainerTopicDetail.TopicName = reader["TopicName"].ToString();
        //                trainerTopicDetail.UserName = reader["UserName"].ToString();
        //            }
        //            connection.Close(); // ngat ket noi
        //        }
        //    }
        //    return ;
        //}
    }
}
