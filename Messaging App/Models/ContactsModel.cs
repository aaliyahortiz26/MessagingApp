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
    public class ContactsModel : DBObject
    {
        public string addContactInput { get; set; }

        public static List<string> m_contactslist;
        public static string m_contactsName;

        public void SetContactsListAttr(List<string> contactslist)
        {
            m_contactslist = contactslist;
        }

    }
}
