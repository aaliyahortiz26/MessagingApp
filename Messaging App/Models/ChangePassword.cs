using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public class ChangePassword
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
