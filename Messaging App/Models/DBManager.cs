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

		public List<string> GetDiscussionQuestionTopic(string name2)
		{
			DBObject.m_TopicName = name2;
			List<string> descriptionQuestionList = new List<string>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagestopic = conn.CreateCommand();
				getMessagestopic.CommandText = "SELECT description, topicQuestion FROM topics where topicName = @chatName";
				getMessagestopic.Parameters.AddWithValue("@chatName", name2);
				getMessagestopic.ExecuteNonQuery();

				MySqlDataReader reader = getMessagestopic.ExecuteReader();
				while (reader.Read())
				{
					descriptionQuestionList.Clear();
					descriptionQuestionList.Add(Convert.ToString(reader[0]));
					descriptionQuestionList.Add(Convert.ToString(reader[1]));
				}
				reader.Close();
				conn.Close();
			}
			return descriptionQuestionList;
		}
		public List<Messages> GetMessagesTopic(string name2)
		{
			DBObject.m_TopicName = name2;
			List<string> messageDataList = new List<string>();
			List<Messages> messageData = new List<Messages>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagestopic = conn.CreateCommand();
				getMessagestopic.CommandText = "SELECT userName, topicMessage FROM messagetopicbase where topicName = @chatName";
				getMessagestopic.Parameters.AddWithValue("@chatName", name2);
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
		public List<string> GettopicUsers(string name2)
		{
			DBObject.m_TopicName = name2;
			List<string> userTopicList = new List<string>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getMessagestopic = conn.CreateCommand();
				getMessagestopic.CommandText = "SELECT contactName FROM topics where topicName = @chatName"; // the command
				getMessagestopic.Parameters.AddWithValue("@chatName", name2);

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

		// return a list of user's contact for home screen
		public List<string> GetUserContacts()
		{
			List<string> userContactList = new List<string>();

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getContacts = conn.CreateCommand();
				getContacts.CommandText = "SELECT username_newContact FROM contacts where UserId = @userID"; // the command
				getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getContacts.ExecuteReader();
				while (reader.Read())
				{
					userContactList.Add(Convert.ToString(reader[0]));
				}
				reader.Close();
				conn.Close();
			}
			return userContactList;
		}
		public Dictionary<string, int> GetTopTopics()
		{
			string privacyOption = "public";
			List<string> userContactList = new List<string>();
			List<string> topicsList = new List<string>();
			List<string> sortedTopicList = new List<string>();
			Dictionary<string, int> topicDictionary = new Dictionary<string, int>();
			int i = 0;

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getTopics = conn.CreateCommand();
				getTopics.CommandText = "SELECT topicName FROM topics where privacyOption = @privacyOption"; // the command
				getTopics.Parameters.AddWithValue("@privacyOption", privacyOption);

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getTopics.ExecuteReader();
				while (reader.Read())
				{	
					if (i != 0)
                    {
						if (Convert.ToString(reader[0]) == topicsList[i - 1])
						{							
						}
						else
						{
							topicsList.Add(Convert.ToString(reader[0]));
							i++;
						}
					}
					else
                    {
						topicsList.Add(Convert.ToString(reader[0]));
						i++;
					}
				}
				reader.Close();




				for (int counter= 0; counter < topicsList.Count(); counter++)
                {
					MySqlCommand getNumMessages = conn.CreateCommand();
					getNumMessages.CommandText = "SELECT count(*) FROM messagetopicbase where topicName= @topicName"; // the command
					getNumMessages.Parameters.AddWithValue("@topicName", topicsList[counter]);

					int numMessages = Convert.ToInt32(getNumMessages.ExecuteScalar());
					topicDictionary.Add(topicsList[counter], numMessages);
				}

				conn.Close();
			}

			Dictionary<string, int> sortedDict = (from entry in topicDictionary orderby entry.Value descending select entry).Take(3).ToDictionary(pair => pair.Key, pair => pair.Value);
			return sortedDict;
		}
	}
}
