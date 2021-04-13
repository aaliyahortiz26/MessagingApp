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
    public class TopicTemplateModel : DBObject
    {
        public static string m_topicName;
        public static string m_question;
        public static string m_description;

        public string topicName { get=> m_topicName; set=>m_topicName = value; }

        //public string description { get => m_description; set => m_description = value; }

        //public string question { get => m_question; set => m_question = value; }


        public static List<string> m_topiclist;


        public void SettopicListAttr(List<string> topiclist)
        {
            m_topiclist = topiclist;
        }

        public string message { get; set; }

    }

}
