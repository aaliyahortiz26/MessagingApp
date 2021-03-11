using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public abstract class DBObject
    {
        public static string m_username;
        public static int m_id;

        public string Username
        {
            get => m_username;
            set => m_username = value;
        }
        public int ID
        {
            get => m_id;
            set => m_id = value;
        }
    }
}
