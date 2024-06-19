using System.ComponentModel.DataAnnotations;

namespace Appointement.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage ="New password and comfirm password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
