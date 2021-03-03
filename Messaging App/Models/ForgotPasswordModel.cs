using System.ComponentModel.DataAnnotations;
namespace MessagingApp.Models
{
    public class ForgotPasswordModel
    {     
        [Required(ErrorMessage = "Required field.")]
        [Display(Name = "Email:")]
        [EmailAddress]
        [StringLength(32, MinimumLength = 0)]
        public string Email { get; set; }
        
        public string SecurityCode { get; set; }      
    }
}