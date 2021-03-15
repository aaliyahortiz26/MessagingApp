using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public class AccountSettings:DBObject
    {

        [Display(Name = "Username")]
        public string userName { get; set; }

        [Display(Name = "Email:")]
        [EmailAddress]
        [StringLength(32, MinimumLength = 0)]
        public string Email { get; set; }
    }
}
