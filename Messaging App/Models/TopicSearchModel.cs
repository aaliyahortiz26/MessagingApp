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

    public class TopicSearchModel
    {
        [Required(ErrorMessage = "Required Field")]
        public string categoryDropdown { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public string topicDropdown { get; set; }

        public static List<string> topicSearchList;
        public void SetTopicsListAttr(List<string> topicList)
        {
            topicSearchList = topicList;
        }
    }
}