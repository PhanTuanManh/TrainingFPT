using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TrainingFPT.Validations;

namespace TrainingFPT.Models
{
    public class UserViewModel
    {
        public List<UserDetail> UserDetailList { get; set; }
    }
    public class UserDetail
    {
        public int Id { get; set; } 
        public int RoleId {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Choose File, please")]
        [AllowExtensionFile(new string[] { ".png", ".jpg", ".jpeg" })]
        [AllowMaxSizeFile(5 * 1024 * 1024)]
        public IFormFile AvatarFile { get; set; }

        public string Phone { get; set; }
        public string Address {  get; set; }
        public DateTime BirthDay {  get; set; }
        public string Gender {  get; set; }
        public string ExtraCode { get; set; }
        [AllowNull]
        public string? Avatar {  get; set; }
        [AllowNull]
        public string? Education {  get; set; }
        [AllowNull]
        public string? ProgramingLang {  get; set; }
        [AllowNull]
        public int ToeicScore {  get; set; }
        [AllowNull]
        public string? Skills { get; set; }
        [AllowNull]
        public string? IpClient {  get; set; }
        [AllowNull]
        public DateTime? LastLogin { get; set; }
        [AllowNull]
        public DateTime? LastLogout { get; set; }
        public string Status { get; set; }
        [AllowNull]
        public DateTime? CreatedAt { get; set; }

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [AllowNull]
        public DateTime? DeletedAt { get; set; }
        [AllowNull]
        public string? NameRole { get; set; }
    }
}
