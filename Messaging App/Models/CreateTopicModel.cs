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

    public class CreateTopicModel
    {
        [Required(ErrorMessage = "Required Field")]
        public string topicName { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string description { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string question { get; set; }

        public string inviteContact { get; set; }

        public string radioField { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string category { get; set; }

    }
}