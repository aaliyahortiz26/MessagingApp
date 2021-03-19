using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public abstract class DBObject
    {
        public static string m_username;
        public static string m_email;
        public static string RandC;
        public static int m_id;
        public static string Bcolor;
        public static string Tcolor;

        public string Username
        {
            get => m_username;
            set => m_username = value;
        }
        public string email
        {
            get => m_email;
            set => m_email = value;
        }
        public int ID
        {
            get => m_id;
            set => m_id = value;
        }
        public string RandC2
        {
            get => RandC;
            set => RandC = value;
        }
        public string Tcolor1
        {
            get => Tcolor;
            set => Tcolor = value;
        }
        public string Bcolor1
        {
            get => Bcolor;
            set => Bcolor = value;
        }
    }
}
