using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MessagingApp.Models
{
	public class DBManager
	{
		public List<Messages> GetMessagesGroup(string name)
		{
			DBObject.m_GroupName = name;
			List<string> messageDataList = new List<string>();
			List<Messages> messageData = new List<Messages>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagesGroup = conn.CreateCommand();
				getMessagesGroup.CommandText = "SELECT userName, groupmessage FROM groupmessagetext where chatName = @chatName"; // the command
				getMessagesGroup.Parameters.AddWithValue("@chatName", name);
				getMessagesGroup.ExecuteNonQuery();

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getMessagesGroup.ExecuteReader();
				while (reader.Read())
				{
					messageDataList.Clear();
					messageDataList.Add(Convert.ToString(reader[0]));
					messageDataList.Add(Convert.ToString(reader[1]));
					messageData.Add(new Messages(messageDataList));
				}
				reader.Close();
				conn.Close();
			}
			return messageData;
		}
		public List<string> GetGroupUsers(string name)
		{
			DBObject.m_GroupName = name;
			List<string> userGroupList = new List<string>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagesGroup = conn.CreateCommand();
				getMessagesGroup.CommandText = "SELECT contactName FROM groupmessage where chatName = @chatName"; // the command
				getMessagesGroup.Parameters.AddWithValue("@chatName", name);

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getMessagesGroup.ExecuteReader();
				while (reader.Read())
				{
					userGroupList.Add(Convert.ToString(reader[0]));
				}
				reader.Close();
				conn.Close();
			}
			return userGroupList;
		}

		public List<Messages> GetMessagesTopic(string name)
		{
			DBObject.m_TopicName = name;
			List<string> messageDataList = new List<string>();
			List<Messages> messageData = new List<Messages>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagestopic = conn.CreateCommand();
				getMessagestopic.CommandText = "SELECT userName, topics FROM messagetopicbase where topicName = @chatName"; // the command
				getMessagestopic.Parameters.AddWithValue("@chatName", name);
				getMessagestopic.ExecuteNonQuery();

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getMessagestopic.ExecuteReader();
				while (reader.Read())
				{
					messageDataList.Clear();
					messageDataList.Add(Convert.ToString(reader[0]));
					messageDataList.Add(Convert.ToString(reader[1]));
					messageData.Add(new Messages(messageDataList));
				}
				reader.Close();
				conn.Close();
			}
			return messageData;
		}
		public List<string> GettopicUsers(string name)
		{
			DBObject.m_TopicName = name;
			List<string> userTopicList = new List<string>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagestopic = conn.CreateCommand();
				getMessagestopic.CommandText = "SELECT contactName FROM message where topicName = @chatName"; // the command
				getMessagestopic.Parameters.AddWithValue("@chatName", name);

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getMessagestopic.ExecuteReader();
				while (reader.Read())
				{
					userTopicList.Add(Convert.ToString(reader[0]));
				}
				reader.Close();
				conn.Close();
			}
			return userTopicList;
		}

	}
}
