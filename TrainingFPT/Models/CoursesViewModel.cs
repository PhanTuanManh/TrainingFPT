using System.ComponentModel.DataAnnotations;
using TrainingFPT.Validations;

namespace TrainingFPT.Models
{
    public class CoursesViewModel
    {
        public List<CourseDetail> CourseDetailList { get; set; }

    }
    public class CourseDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Choose Category, please")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Enter name's course, please")]
        public string NameCourse { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Enter start date, please")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Choose file image, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg", ".gif" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }

        public string? ViewImageCouser { get; set; }

        public int? LikeCourse { get; set; }

        public int? StarCourse { get; set; }
        public string? NameCategory { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }

        public string? viewCategoryName { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}