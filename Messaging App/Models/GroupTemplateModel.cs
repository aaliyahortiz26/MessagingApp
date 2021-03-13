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
    public class GroupTemplateModel : DBObject
    {
        public static List<string> m_grouplist;
        public static string m_groupName;

        //        List<string> groupList { get => m_grouplist; set => m_grouplist = value; }

        public void SetGroupListAttr(List<string> grouplist)
        {
            m_grouplist = grouplist;
        }

        public string message {get; set;}

        public string groupName { get=> m_groupName; set=>m_groupName = value; }

    }

}
