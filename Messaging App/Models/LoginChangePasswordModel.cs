using System.ComponentModel.DataAnnotations;
namespace MessagingApp.Models
{
    public class LoginChangePasswordModel
    {                    
        [Required(ErrorMessage = "Required field. *")]
        [DataType(DataType.Password)]
 //       [Compare("ConfirmPassword")]
        [Display(Name = "New Password:")]
        [StringLength(32, MinimumLength = 0)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required. *")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        [StringLength(32, MinimumLength = 0)]
        [Compare("NewPassword", ErrorMessage = "Passwords are not the same. *")]
        public string ConfirmPassword { get; set; }      
    
        public string userName { get; set; }
    }
}