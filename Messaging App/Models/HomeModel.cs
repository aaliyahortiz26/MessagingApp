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
    public class HomeModel : DBObject
    {
        public static List<string> m_grouplist;
        public void SetGroupListAttr(List<string> grouplist)
        {
            m_grouplist = grouplist;
        }

        public static List<string> m_contactslist;
        public void SetcontactsListAttr(List<string> contactslist)
        {
            m_contactslist = contactslist;
        }

        public static List<string> m_topiclist;
        public void SetttopicListAttr(List<string> topiclist)
        {
            m_topiclist = topiclist;
        }

    }

}
