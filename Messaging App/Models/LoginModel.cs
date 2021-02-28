using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models
{

    public class LoginModel
    {
        [Required(ErrorMessage = "Enter Your UserName")]
        [Display(Name = "UserName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter Your Password")]
        [Display(Name = "Pass")]
        public string Pass { get; set; }

    }

}
