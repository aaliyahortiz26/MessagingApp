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
        public static string topic;
        public static string description;
        public static string question;
        public static string category;
        public static string contactName;
        public string radioField { get; set; }

        public string m_contactName
        {
            get => contactName;
            set => contactName = value;
        }
        public string m_category
        {
            get => category;
            set => category = value;
        }
        public string m_topic
        {
            get => topic;
            set => topic = value;
        }
        public string m_description
        {
            get => description;
            set => description = value;
        }

        public string m_question
        {
            get => question;
            set => question = value;
        }
        //[Required(ErrorMessage = "Required Field")]
        public string categoryDropdown { get; set; }

        //[Required(ErrorMessage = "Required Field")]
        public string topicDropdown { get; set; }

        //public string description { get; set; }
        //public string question { get; set; }

        public static List<string> topicSearchList;
        public void SetTopicsListAttr(List<string> topicList)
        {
            topicSearchList = topicList;
        }

        public static List<string> topicCategoryList;
        public void SetCategoryListAttr(List<string> categoryList)
        {
            topicCategoryList = categoryList;
        }

        public static List<string> m_memberlist;
        public void SetPinnedListAttr(List<string> memberlist)
        {
            m_memberlist = memberlist;
        }

        public static List<string> m_descriptionlist;
        public void SetuserPinnedListAttr(List<string> descriptionlist)
        {
            m_descriptionlist = descriptionlist;
        }
    }
}