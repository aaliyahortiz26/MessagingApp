﻿using System;
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
            cmd.CommandText = $"Insert into groupmessagetext (userId,groupId,groupmessage,chatName)" + $"values ( @userId,@groupId,@groupmessage,@chatName) ";
            cmd.Parameters.AddWithValue("@userId", DBObject.m_id);
            cmd.Parameters.AddWithValue("@groupId", DBObject.m_Groupid);
            cmd.Parameters.AddWithValue("@groupmessage", message );
            cmd.Parameters.AddWithValue("@chatName", DBObject.m_GroupName);
            cmd.Prepare();
            cmd.ExecuteReader();
            conn.Close();
            dRead.Close();


        }
    }
}
