using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MessagingApp.Models
{
    public class Messages
    {
        private string userName;
        private string message;
        private string messageID;
        private string userID;
        private string image = "";


        public Messages(List<string> messageData)
		{
            userName = messageData[0];
            message = messageData[1];
            messageID = messageData[2];
            userID = messageData[3];
            image = messageData[4];
        }
        public string GetUsername()
        {
            return userName;
        }
        public string GetMessage()
        {
            return message;
        }
        public string GetMessageID()
        {
            return messageID;
        }
        public string GetUserID()
        {
            return userID;
        }
        public string GetImage()
        {
            return image;
        }
        public string AlreadySaved(string messagingType, string username)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();
            int numMessages = 0;
            if (messagingType == "Topic")
            {                                    
                if (message != "" && image != "")
                {
                    string txtcmd1 = "SELECT count(*) FROM pinnedMessages where (userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicgroupName = @topicGroupName and image = @image)";
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd1.Parameters.AddWithValue("@userName", username);
                    cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                    cmd1.Parameters.AddWithValue("@topicGroupName", DBObject.m_TopicName);
                    cmd1.Parameters.AddWithValue("@image", image);

                    numMessages = Convert.ToInt32(cmd1.ExecuteScalar());
                }
                else if (message == "" && image != "")
                {
                    string txtcmd1 = "SELECT count(*) FROM pinnedMessages where (userId = @userID and userName = @userName and topicgroupName = @topicGroupName and image = @image)";
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd1.Parameters.AddWithValue("@userName", username);
                    cmd1.Parameters.AddWithValue("@topicGroupName", DBObject.m_TopicName);
                    cmd1.Parameters.AddWithValue("@image", image);

                    numMessages = Convert.ToInt32(cmd1.ExecuteScalar());
                }
                else if (message != "" && image == "")
                {
                    string txtcmd1 = "SELECT count(*) FROM pinnedMessages where (userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicGroupName = @topicGroupName)";
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd1.Parameters.AddWithValue("@userName", username);
                    cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                    cmd1.Parameters.AddWithValue("@topicGroupName", DBObject.m_TopicName);

                    numMessages = Convert.ToInt32(cmd1.ExecuteScalar());
                }


                if (numMessages > 0)
                {
                    return "Already saved.";
                }
                               
            }
            else
            {
                if (message != "" && image != "")
                {
                    string txtcmd1 = "SELECT count(*) FROM pinnedMessages where (userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicgroupName = @topicGroupName and image = @image)";
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd1.Parameters.AddWithValue("@userName", username);
                    cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                    cmd1.Parameters.AddWithValue("@topicGroupName", DBObject.m_GroupName);
                    cmd1.Parameters.AddWithValue("@image", image);

                    numMessages = Convert.ToInt32(cmd1.ExecuteScalar());
                }
                else if (message == "" && image != "")
                {
                    string txtcmd1 = "SELECT count(*) FROM pinnedMessages where (userId = @userID and userName = @userName and topicgroupName = @topicGroupName and image = @image)";
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd1.Parameters.AddWithValue("@userName", username);
                    cmd1.Parameters.AddWithValue("@topicGroupName", DBObject.m_GroupName);
                    cmd1.Parameters.AddWithValue("@image", image);

                    numMessages = Convert.ToInt32(cmd1.ExecuteScalar());
                }
                else if (message != "" && image == "")
                {
                    string txtcmd1 = "SELECT count(*) FROM pinnedMessages where (userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicGroupName = @topicGroupName)";
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd1.Parameters.AddWithValue("@userName", username);
                    cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                    cmd1.Parameters.AddWithValue("@topicGroupName", DBObject.m_GroupName);

                    numMessages = Convert.ToInt32(cmd1.ExecuteScalar());
                }

                if (numMessages > 0)
                {
                    return "Already Saved";
                }
            }
            return "Save";
            
        }
    /*    public string GetPinnedMessageID(string messagingType)
        {
            string pinnedID = "";
            if (messagingType == "Topic")
            {
                const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                MySqlConnection conn = new MySqlConnection(connectionstring);

                conn.Open();

                string txtcmd1 = "select pinnedMessagesNum FROM pinnedMessages where userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicgroupName = @topicName and image = @image";
                MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                cmd1.Parameters.AddWithValue("@userName", DBObject.m_username);
                cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                cmd1.Parameters.AddWithValue("@topicName", DBObject.m_TopicName);
                cmd1.Parameters.AddWithValue("@image", image);

                MySqlDataReader mRead;
                using (mRead = cmd1.ExecuteReader()) // executes the search command
                {
                    if (mRead.Read())
                    {
                        pinnedID = Convert.ToString(mRead[0]);
                    }
                }
            }
            return pinnedID;

        }*/
    }
}
