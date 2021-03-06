﻿using System.ComponentModel.DataAnnotations;
namespace MessagingApp.Models
{
    public class CreateAccountModel
    {
        [StringLength(32, MinimumLength = 0)]
        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "First Name:")]
        
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Last Name:")]
        [StringLength(32, MinimumLength = 0)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Username:")]
        [StringLength(32, MinimumLength = 0)]
        public string UserName { get; set; }

        // [Required(ErrorMessage = "Phone has an invalid format. Format: ###-###-####")")]
        [Display(Name = "Phone Number:")]
        [Phone(ErrorMessage = "Not a valid phone number. *")]
        public string Phone_Number { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [Display(Name = "Email Address:")]
        [EmailAddress]
        [StringLength(32, MinimumLength = 0)]
        public string Email { get; set; }

        [Display(Name = "Date Of Birth:")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Required field. *")]
        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        [StringLength(32, MinimumLength = 0)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required. *")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        [StringLength(32, MinimumLength = 0)]
        [Compare("Password", ErrorMessage = "Passwords are not the same. *")]
        public string ConfirmPassword { get; set; }      
    }
}