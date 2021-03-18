using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MessagingApp.Models
{
        public class AccountSettings : DBObject
        {

            public string userName { get; set; }

            public string Email { get; set; }
        }
}
