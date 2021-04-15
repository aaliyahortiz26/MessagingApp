using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Models
{
    public class Messages
    {
        private string userName;
        private string message;
        private string messageID;



        public Messages(List<string> messageData)
		{
            userName = messageData[0];
            message = messageData[1];
            messageID = messageData[2];
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
    }
}
