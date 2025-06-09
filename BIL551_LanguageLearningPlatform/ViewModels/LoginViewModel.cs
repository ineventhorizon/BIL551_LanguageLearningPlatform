using System.ComponentModel.DataAnnotations;

namespace BIL551_LanguageLearningPlatform.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
