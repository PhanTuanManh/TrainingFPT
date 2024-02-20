using Microsoft.EntityFrameworkCore;

namespace TrainingFPT.DBContext
{
    public class TraningDBContext : DbContext
    {
        public TraningDBContext(DbContextOptions<TraningDBContext> options) : base(options) { }

        // DbSet : tao bang du lieu tu RolesDBContext
        // Roles : ten bang se duoc tao trong databases
        public DbSet<RolesDBContext> Roles { get; set; }
        public DbSet<CategoriesDBContext> Categories { get; set; }
        public DbSet<CourseDBContext> Courses { get; set; }
        public DbSet<TopicsDBContext> Topics { get; set; }
    }
}
