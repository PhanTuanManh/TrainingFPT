using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class CategoryQuery
    {
        // viet method insert category
        public int InsertItemCategory(
            string nameCategory,
            string description,
            string image,
            string status
        ) {
            int lastIdInsert = 0; // id cua category vua dc them moi
            string sqlQuery = "INSERT INTO [Categories]([Name],[Description],[PosterImage],[ParentId], [Status], [CreatedAt ]) VALUES(@nameCategory, @description, @image, @parentId, @status, @createdAt) SELECT SCOPE_IDENTITY()";
            // SELECT SCOPE_IDENTITY() : lay ra id vua dc them moi
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand( sqlQuery, connection );
                cmd.Parameters.AddWithValue("@nameCategory", nameCategory ?? DBNull.Value.ToString() );
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@image", image ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@parentId", 0);
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                lastIdInsert = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
            // lastIdInsert tra ve lon hon 0 insert thanh cong va nguoc lai
            return lastIdInsert;
        }

        public List<CategoryDetail> GetAllCategories()
        {
            List<CategoryDetail> category = new List<CategoryDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Categories]";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryDetail categoryDetail = new CategoryDetail();
                        categoryDetail.Id = Convert.ToInt32(reader["Id"]);
                        categoryDetail.Name = reader["Name"].ToString();
                        categoryDetail.Description = reader["Description"].ToString();
                        categoryDetail.PosterNameImage = reader["PosterImage"].ToString();
                        categoryDetail.Status = reader["Status"].ToString();
                        categoryDetail.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                        category.Add(categoryDetail);
                    }
                    conn.Close();
                }
            }
            return category;
        }
    }
}
