﻿using System;
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
    public class InviteContactTopicModel : DBObject
    {
        [Required(ErrorMessage = "Required Field")]

        public string radioField { get; set; }

        public string inviteContact { get; set; }

    }
}
