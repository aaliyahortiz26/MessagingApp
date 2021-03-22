using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public class AccountSettings : DBObject
    {
        public string userName { get; set; }

        public string Email { get; set; }
    }
}