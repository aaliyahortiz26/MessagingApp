using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessagingApp.Models;
using Microsoft.AspNetCore.SignalR;
using MySql.Data.MySqlClient;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            //string groupName = DBObject.m_GroupName;
            await Clients.All.SendAsync("ReceiveMessage", DBObject.m_username, message);

            // insert into database
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            string txtcmd2 = $"SELECT groupmessageId FROM groupmessage where chatName='" + DBObject.m_GroupName + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
            MySqlDataReader dRead;
            using (dRead = cmd2.ExecuteReader()) // executes the search command
            {
                if (dRead.Read())
                {
                  DBObject.m_Groupid = Convert.ToInt32(dRead.GetValue(0));
                }
            }
            MySqlCommand cmd = new MySqlCommand(null, conn);
            cmd.CommandText = $"Insert into groupmessagetext (userId,userName,groupId,groupmessage,chatName)" + $"values ( @userId,@userName,@groupId,@groupmessage,@chatName) ";
            cmd.Parameters.AddWithValue("@userId", DBObject.m_id);
            cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
            cmd.Parameters.AddWithValue("@groupId", DBObject.m_Groupid);
            cmd.Parameters.AddWithValue("@groupmessage", message );
            cmd.Parameters.AddWithValue("@chatName", DBObject.m_GroupName);
            cmd.Prepare();
            cmd.ExecuteReader();
            conn.Close();
            dRead.Close();


        }
        public async Task SendMessageTopic(string message)
        {
            // string groupName = DBObject.m_GroupName;
            await Clients.All.SendAsync("ReceiveMessage", DBObject.m_username, message);

            // insert into database
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            string txtcmd2 = $"SELECT topicId FROM topics where chatName='" + DBObject.m_GroupName + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
            MySqlDataReader dRead;
            using (dRead = cmd2.ExecuteReader()) // executes the search command
            {
                if (dRead.Read())
                {
                    DBObject.m_Topicid = Convert.ToInt32(dRead.GetValue(0));
                }
            }
            MySqlCommand cmd = new MySqlCommand(null, conn);
            cmd.CommandText = $"Insert into messagetopicbase (userId,userName,topicId,topicMessage,topicName)" + $"values ( @userId,@userName,@topicId,@topicMessage,@topicName) ";
            cmd.Parameters.AddWithValue("@userId", DBObject.m_id);
            cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
            cmd.Parameters.AddWithValue("@topicId", DBObject.m_Topicid);
            cmd.Parameters.AddWithValue("@topicMessage", message);
            cmd.Parameters.AddWithValue("@topicName", DBObject.m_TopicName);
            cmd.Prepare();
            cmd.ExecuteReader();
            conn.Close();
            dRead.Close();
        }
    }
}
