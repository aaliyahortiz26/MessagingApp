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

        public static List<string> m_contactsListHomeScreen;
        public void SetContactListAttr(List<string> contactlist)
        {
            m_contactsListHomeScreen = contactlist;
        }

        public static List<string> m_Pinnedlist;
        public void SetPinnedListAttr(List<string> Pinnedlist)
        {
            m_Pinnedlist = Pinnedlist;
        }
        public static List<string> m_userPinnedlist;
        public void SetuserPinnedListAttr(List<string> userPinnedlist)
        {
            m_userPinnedlist = userPinnedlist;
        }

        public static List<string> m_groupPinnedlist;
        public void SetgroupPinnedListAttr(List<string> groupPinnedlist)
        {
            m_groupPinnedlist = groupPinnedlist;
        }

    }

}
