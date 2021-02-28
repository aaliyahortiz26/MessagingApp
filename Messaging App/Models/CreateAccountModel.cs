using System.ComponentModel.DataAnnotations;
namespace MessagingApp.Models
{
    public class CreateAccountModel
    {

        [Required(ErrorMessage = "Enter Your First Name")]
        [Display(Name = "firstName")]
        [StringLength(32)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter Your Last Name")]
        [Display(Name = "lastName")]
        [StringLength(32)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter Your UserName")]
        [Display(Name = "UserName")]
        [StringLength(32)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter Your Phone Number")]
        [Display(Name = "Phone Number")]
        public string Phone_Number { get; set; }

        [Required(ErrorMessage = "Enter Your Email")]
        [Display(Name = "Email Adress")]
        [EmailAddress(ErrorMessage = "Please Enter a valid Email Adress")]
        [StringLength(32)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Your Date Of Birth")]
        [Display(Name = "Date Of Birth")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enter Your Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [Display(Name = "Password")]
        [StringLength(32)]
        public string Password { get; set; }

        [Required(ErrorMessage = " Please Confirm Password")]
        [Display(Name = "Confirm  Password")]
        [StringLength(32)]
        public string ConfirmPassword { get; set; }


    }
}