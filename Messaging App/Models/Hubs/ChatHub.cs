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
            await Clients.All.SendAsync("ReceiveMessage", DBObject.m_username, message);
           
         /*   const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand getContacts = conn.CreateCommand();
            getContacts.CommandText = "insert backgroundColor, fontColor FROM preferences where id = @userID"; // the command
            getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader reader2 = getContacts.ExecuteReader();*/
        }
    }
}
