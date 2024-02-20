using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPT.DBContext
{
    public class TopicsDBContext
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("CoursesId"), Required]
        public required CourseDBContext Courses { get; set; }

        [Column("NameTopic", TypeName = "Varchar(100)"), Required]
        public required string NameTopic { get; set; }

        [Column("Description", TypeName ="Varchar(200)"), AllowNull]
        public string? Description { get; set; }

        [Column("Status", TypeName = "Varchar(20)"), Required]
        public required string Status { get; set; }

        [Column("Documents", TypeName = "Varchar(200)"), Required]
        public required string Documents { get; set; }

        [Column("AttachFiles", TypeName = "Varchar(200)"), AllowNull]
        public string? AttachFiles { get; set; }

        [Column("TypeDocument", TypeName = "Varchar(20)"), Required]
        public required string TypeDocument { get; set; }

        [Column("PosterTopic", TypeName = "Varchar(200)"), Required]
        public required string PosterTopic { get; set; }

        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }

    }
}
