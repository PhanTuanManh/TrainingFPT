using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TrainingFPT.DBContext
{
    public class CategoriesDBContext
    {
        // tao bang Categories trong database TestTraning
        [Key]
        public int Id { get; set; }

        [Column("NameCategory", TypeName ="Varchar(50)"), Required]
        public required string NameCategory { get; set; }

        [Column("Description", TypeName ="Varchar(200)"), AllowNull]
        public string? Description { get; set; }

        [Column("PosterImage", TypeName="Varchar(200)"), Required]
        public required string PosterImage { get; set; }

        [Column("ParentId", TypeName="integer"),  Required]
        public required int ParentId { get; set;}

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
