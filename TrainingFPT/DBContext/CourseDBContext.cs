using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPT.DBContext
{
    public class CourseDBContext
    {
        [Key]
        public int Id { get; set; }
        
        [ForeignKey("CategoriesId"), Required]
        public required CategoriesDBContext Categories {  get; set; }

        [Column("NameCourse", TypeName ="Varchar(60)"), Required]
        public required string NameCourse { get; set; }

        [Column("Description", TypeName="Varchar(200)"), AllowNull]
        public string? Description { get; set; }

        [Column("Image", TypeName ="Varchar(200)"), Required]
        public required string Image {  get; set; }

        [Column("LikeCourse", TypeName ="integer"), AllowNull]
        public int? LikeCourse { get; set;}

        [Column("StarCourse", TypeName = "integer"), AllowNull]
        public int? StarCourse { get; set; }

        [Column("Status", TypeName = "Varchar(20)"), Required]
        public required string Status { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }
    }
}
