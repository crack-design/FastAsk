using System.ComponentModel.DataAnnotations;

namespace Authentication.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
