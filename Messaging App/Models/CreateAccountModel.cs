using System.ComponentModel.DataAnnotations;
namespace MessagingApp.Models
{
    public class CreateAccountModel
    {

        [Required]
        [Display(Name = "firstName")]
        [StringLength(32, ErrorMessage = "First length can't be more than 32.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "lastName")]
        [StringLength(32, ErrorMessage = "LastName length can't be more than 32.")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "UserName")]
        [StringLength(32, ErrorMessage = "UserName length can't be more than 32.")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Phone has an invalid format. Format: ###-###-####")]
        public string Phone_Number { get; set; }

        [Required]
        [Display(Name = "Email Adress")]
        [EmailAddress(ErrorMessage = "Please Enter a valid Email Adress")]
        [StringLength(32, ErrorMessage = "Email length can't be more than 32.")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]
        public string DateOfBirth { get; set; }

        [Required]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [Display(Name = "Password")]
        [StringLength(32, ErrorMessage = "Password length can't be more than 32.")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm  Password")]
        [StringLength(32, ErrorMessage = "Confirm Password length can't be more than 32.")]
        public string ConfirmPassword { get; set; }


    }
}