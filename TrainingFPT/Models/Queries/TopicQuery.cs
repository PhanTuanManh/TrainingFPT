using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class TopicQuery
    {
        public List<TopicDetail> GetAllTopics(string? keyword, string? filterStatus)
    {
        string dataKeyword = "%" + keyword + "%";
        List<TopicDetail> Topic = new List<TopicDetail>();
        Dictionary<int, string> courseNames = new Dictionary<int, string>();

        using (SqlConnection conn = Database.GetSqlConnection())
        {
            string sqlQuery = string.Empty;
                sqlQuery = "SELECT * FROM [Topics] WHERE [NameTopic] LIKE @keyword AND [DeletedAt] IS NULL";
                if (filterStatus != null)
                {
                    sqlQuery += " AND [Status] = @status";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());
            if (filterStatus != null)
            {
                cmd.Parameters.AddWithValue("@status", filterStatus ?? DBNull.Value.ToString());
            }


            conn.Open();
            using (SqlCommand cmdCourses = new SqlCommand("SELECT Id, NameCourse FROM Courses", conn))
            {
                using (SqlDataReader readerCourses = cmdCourses.ExecuteReader())
                {
                    while (readerCourses.Read())
                    {
                        courseNames.Add(Convert.ToInt32(readerCourses["Id"]), readerCourses["NameCourse"].ToString());
                    }
                }
            }
            conn.Close();


            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    TopicDetail topicDetail = new TopicDetail();
                    topicDetail.Id = Convert.ToInt32(reader["Id"]);
                    topicDetail.CourseId = Convert.ToInt32(reader["CouresId"]);
                    topicDetail.Name = reader["NameTopic"].ToString();
                    topicDetail.Description = reader["Description"].ToString();
                    topicDetail.NameVideo = reader["Video"].ToString();
                    topicDetail.NameAudio = reader["Audio"].ToString();
                    topicDetail.DocumentNameTopic = reader["DocumentTopic"].ToString();
                    topicDetail.Status = reader["Status"].ToString();
                    topicDetail.Like = Convert.ToInt32(reader["LikeTopic"]);
                    topicDetail.Star = Convert.ToInt32(reader["StarTopic"]);
                    topicDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                    topicDetail.UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : DateTime.MinValue;
                    topicDetail.DeletedAt = reader["DeletedAt"] != DBNull.Value ? Convert.ToDateTime(reader["DeletedAt"]) : DateTime.MinValue;
                    if (courseNames.ContainsKey(topicDetail.CourseId))
                    {
                        topicDetail.NameCourse = courseNames[topicDetail.CourseId];
                    }
                    Topic.Add(topicDetail);

                }
                conn.Close();
            }

        }
        return Topic;
    }

   
        public int InsertItemTopic(
          string name,
          int courseId,
          string description,
          string video,
          string audio,
          string documentTopic,
          int like,
          int star,
          string status
       )
        {
            int lastIdInsert = 0; // id cua category vua dc them moi
            string sqlQuery = "INSERT INTO [Topics]([NameTopic],[CouresId],[Description],[Video],[Audio],[DocumentTopic],[LikeTopic], [StarTopic], [Status], [CreatedAt]) VALUES(@nameTopic, @courseId, @description, @video, @audio, @documentTopic, @like, @star, @status, @createdAt) SELECT SCOPE_IDENTITY()";
            // SELECT SCOPE_IDENTITY() : lay ra id vua dc them moi

            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@nameTopic", name ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@courseId", courseId);
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@video", video ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@audio", audio ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@documentTopic", documentTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@like", like);
                cmd.Parameters.AddWithValue("@star", star);
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastIdInsert = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }

            // lastIdInsert tra ve lon hon 0 insert thanh cong va nguoc lai
            return lastIdInsert;
        }
        public List<TopicDetail> GetCourse()
        {
            List<TopicDetail> topics = new List<TopicDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Courses] WHERE [DeletedAt] IS NULL";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Kiểm tra xem có dữ liệu để đọc hay không
                    {
                        TopicDetail topicDetail = new TopicDetail();
                        topicDetail.NameCourse = reader["NameCourse"].ToString();
                        topicDetail.CourseId = Convert.ToInt32(reader["Id"]);
                        topics.Add(topicDetail);

                    }
                }
                conn.Close();
            }
            return topics;
        }
        public TopicDetail GetDataTopicById(int id = 0)
        {
            TopicDetail topicDetail = new TopicDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Topics] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topicDetail.Id = Convert.ToInt32(reader["Id"]);
                        topicDetail.CourseId = Convert.ToInt32(reader["CouresId"]);
                        topicDetail.Name = reader["NameTopic"].ToString();
                        topicDetail.Description = reader["Description"].ToString();
                        topicDetail.NameVideo = reader["Video"].ToString();
                        topicDetail.NameAudio = reader["Audio"].ToString();
                        topicDetail.DocumentNameTopic = reader["DocumentTopic"].ToString();
                        topicDetail.Status = reader["Status"].ToString();
                        topicDetail.Like = Convert.ToInt32(reader["LikeTopic"]);
                        topicDetail.Star = Convert.ToInt32(reader["StarTopic"]);
                        topicDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        topicDetail.UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedAt"]) : DateTime.MinValue;
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return topicDetail;
        }
        public bool UpdateTopicById(
           string nameTopic,
           int courseId,
           string description,
           string video,
           string audio,
           string documentTopic,
           int like,
           int star,
           string status,
           int id
       )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Topics] SET [NameTopic] = @nameTopic,[CouresId] = @courseId, [Description] = @description, [Video] = @video,[Audio] = @audio, [DocumentTopic] = @documentTopic, [LikeTopic] = @like, [StarTopic] = @star, [Status] = @status, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
                cmd.Parameters.AddWithValue("@nameTopic", nameTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@courseId", courseId);
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@video", video ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@audio", audio ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@documentTopic", documentTopic ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@like", like);
                cmd.Parameters.AddWithValue("@star", star);
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
        public bool DeleteItemTopic(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Topics] SET [DeletedAt] = @deletedAt WHERE [Id] = @id";
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@deletedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
                statusDelete = true;
                connection.Close();
            }
            // false : ko xoa dc - true : xoa thanh cong
            return statusDelete;
        }
    }
}
