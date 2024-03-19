using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;

namespace TrainingFPT.Models.Queries
{
    public class CategoryQuery
    {
        // viet ham - cap nhat lai category
        public bool UpdateCategoryById(
            string nameCategory,
            string description,
            string posterImage,
            string status,
            int id
        )
        {
            bool checkUpdate = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlUpdate = "UPDATE [Categories] SET [Name] = @name, [Description] = @description, [PosterImage] = @posterImage, [Status] = @status, [UpdatedAt] = @updatedAt WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand( sqlUpdate, connection );
                cmd.Parameters.AddWithValue("@name", nameCategory ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@description", description ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@posterImage", posterImage ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@status", status ?? DBNull.Value.ToString());
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                connection.Close();
                checkUpdate = true;
            }
            return checkUpdate;
        }
        // viet ham lay thong tin chi tiet cua Category
        public CategoryDetail GetDataCategoryById(int id = 0)
        {
            CategoryDetail categoryDetail = new CategoryDetail();
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "SELECT * FROM [Categories] WHERE [Id] = @id AND [DeletedAt] IS NULL";
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categoryDetail.Id = Convert.ToInt32(reader["Id"]);
                        categoryDetail.Name = reader["Name"].ToString();
                        categoryDetail.Description = reader["Description"].ToString();
                        categoryDetail.PosterNameImage = reader["PosterImage"].ToString();
                        categoryDetail.Status = reader["Status"].ToString();
                    }
                    connection.Close(); // ngat ket noi
                }
            }
            return categoryDetail;
        }
        // viet method xoa category
        public bool DeleteItemCategory(int id = 0)
        {
            bool statusDelete = false;
            using (SqlConnection connection = Database.GetSqlConnection())
            {
                string sqlQuery = "UPDATE [Categories] SET [DeletedAt] = @deletedAt WHERE [Id] = @id";
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

        public List<CategoryDetail> GetAllCategories(string? keyword, string? filterStatus)
        {
            string dataKeyword = "%" + keyword + "%";
            List<CategoryDetail> category = new List<CategoryDetail>();
            using (SqlConnection conn = Database.GetSqlConnection())
            {
                string sqlQuery = string.Empty;
                if (filterStatus != null)
                {
                    sqlQuery = "SELECT * FROM [Categories] WHERE [Name] LIKE @keyword AND [DeletedAt] IS NULL AND [Status] = @status";
                }
                else
                {
                    sqlQuery = "SELECT * FROM [Categories] WHERE [Name] LIKE @keyword AND [DeletedAt] IS NULL";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.Parameters.AddWithValue("@keyword", dataKeyword ?? DBNull.Value.ToString());
                if (filterStatus != null)
                {
                    cmd.Parameters.AddWithValue("@status", filterStatus ?? DBNull.Value.ToString());
                }
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
