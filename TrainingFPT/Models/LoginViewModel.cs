namespace TrainingFPT.Models
{
    public class LoginViewModel
    {
        public int Id {  get; set; }
        public int? RoleId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set;}
        public string? Extracode { get; set;}
    }
}
