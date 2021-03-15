using System.ComponentModel.DataAnnotations;
namespace MessagingApp.Models
{
    public class ForgotPasswordModel : DBObject
    {
        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Username:")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Email:")]
        [EmailAddress]
        [StringLength(32, MinimumLength = 0)]
        public string Email { get; set; }

        [Display(Name = "Security Code:")]
        public string SecurityCode { get; set; }

    }
}