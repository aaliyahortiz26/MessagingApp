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
        public static int m_TSize;
        public static string m_GroupName;
        public static int m_Groupid;
        public static string m_TopicName;
        public static int m_Topicid;

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
        public int TSize
        {
            get => m_TSize;
            set => m_TSize = value;
        }
        public string GroupName
        {
            get => m_GroupName;
            set => m_GroupName = value;
        }
        public int GroupId
        {
            get => m_Groupid;
            set => m_Groupid = value;
        }
        public string TopicName
        {
            get => m_TopicName;
            set => m_TopicName = value;
        }
        public int TopicId
        {
            get => m_Topicid;
            set => m_Topicid = value;
        }

    }
}
