using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TrainingFPT.Validations;

namespace TrainingFPT.Models
{
    public class TopicViewModel
    {
        public List<TopicDetail> TopicDetailList { get; set; }
    }
    public class TopicDetail
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int CourseId { get; set; }

        [AllowNull]
        public string? Description { get; set; }

       
        [AllowExtensionFile(new string[] { ".mp4", ".avi", ".mov", ".wmv" })]
        [AllowMaxSizeFile(25 * 1024 * 1024)]
        public IFormFile? VideoFile { get; set; }

       
        [AllowExtensionFile(new string[] { ".mp3", ".wav", ".ogg" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)] // 5 MB
        public IFormFile? AudioFile { get; set; }

        
        [AllowExtensionFile(new string[] { ".docx", ".pdf", ".txt" })]
        [AllowMaxSizeFile(10 * 1024 * 1024)] // 5 MB
        public IFormFile? DocumentFile { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string Status { get; set; }
        [AllowNull]
        public int Like { get; set; }
        [AllowNull]
        public int Star { get; set; }
        [AllowNull]
        public string? DocumentNameTopic { get; set; }

        [AllowNull]
        public string? NameVideo { get; set; }
        [AllowNull]
        public string? NameAudio { get; set; }
        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }

        [AllowNull]
        public string? NameCourse { get; set; }
    }
}
