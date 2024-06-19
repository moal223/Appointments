using System.ComponentModel.DataAnnotations;

namespace Appointement.ViewModels
{
    public class SignInViewModel
    {
        [EmailAddress(ErrorMessage ="Email not valid")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Remberme { get; set; }
    }
}
