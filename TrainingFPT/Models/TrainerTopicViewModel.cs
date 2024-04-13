using System.Diagnostics.CodeAnalysis;

namespace TrainingFPT.Models
{
    public class TrainerTopicViewModel
    {
        public List<TrainerTopicDetail> TrainerTopicDetailList { get; set; }
    }
    public class TrainerTopicDetail
    {
        public int UserId { get; set; }
        public int TopicId { get; set; }
        [AllowNull]
        public string? TopicName { get; set; }
        [AllowNull]
        public string? UserName { get; set; }
        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }
        
    }
}
