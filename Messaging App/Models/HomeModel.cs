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

        public static List<string> m_groupinvitelist;
        public void SetGroupinviteListAttr(List<string> groupinvitelist)
        {
            m_groupinvitelist = groupinvitelist;
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
        public static List<string> m_imagePinnedlist;
        public void SetimagePinnedListAttr(List<string> imagePinnedlist)
        {
            m_imagePinnedlist = imagePinnedlist;
        }
        public static List<string> m_messageTypelist;
        public void SetmessageTypePinnedListAttr(List<string> messageTypePinnedlist)
        {
            m_messageTypelist = messageTypePinnedlist;
        }

        public static List<int> m_contactnumber;
        public void SetContactnumberListAttr(List<int> contactnumberlist)
        {
            m_contactnumber = contactnumberlist;
        }
        public static List<int> m_contactSender;
        public void SetContactSenderListAttr(List<int> contactSenderlist)
        {
            m_contactSender = contactSenderlist;
        }

        public static List<String> m_groupuserName;
        public void SetgroupuserNameListAttr(List<String> groupuserNamelist)
        {
            m_groupuserName = groupuserNamelist;
        }

        public static List<int> m_Invite;
        public void SetinviteListAttr(List<int> invitelist)
        {
            m_Invite = invitelist;
        }

        public static List<string> m_InviteGroupContact;
        public void SetinvitegroupContactListAttr(List<string> invitegroupContactlist)
        {
            m_InviteGroupContact = invitegroupContactlist;
        }
    }

}
