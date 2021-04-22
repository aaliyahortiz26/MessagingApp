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
				getMessagesGroup.CommandText = "SELECT userName, groupmessage, Documentid, userId, image FROM groupmessagetext where chatName = @chatName"; // the command
				getMessagesGroup.Parameters.AddWithValue("@chatName", name);
				getMessagesGroup.ExecuteNonQuery();

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getMessagesGroup.ExecuteReader();
				while (reader.Read())
				{
					messageDataList.Clear();
					messageDataList.Add(Convert.ToString(reader[0]));
					messageDataList.Add(Convert.ToString(reader[1]));
					messageDataList.Add(Convert.ToString(reader[2]));
					messageDataList.Add(Convert.ToString(reader[3]));
					messageDataList.Add(Convert.ToString(reader[4]));
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
				getMessagesGroup.CommandText = "SELECT userName FROM groupmessage where chatName = @chatName"; // the command
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

				MySqlCommand getDiscussionQuestionTopic = conn.CreateCommand();
				getDiscussionQuestionTopic.CommandText = "SELECT description, topicQuestion FROM topics where topicName = @chatName";
				getDiscussionQuestionTopic.Parameters.AddWithValue("@chatName", name2);
				getDiscussionQuestionTopic.ExecuteNonQuery();

				MySqlDataReader reader = getDiscussionQuestionTopic.ExecuteReader();
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
				getMessagestopic.CommandText = "SELECT userName, topicMessage, documentId,  userId, image FROM messagetopicbase where topicName = @chatName";
				getMessagestopic.Parameters.AddWithValue("@chatName", name2);
				getMessagestopic.ExecuteNonQuery();

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getMessagestopic.ExecuteReader();
				while (reader.Read())
				{
					messageDataList.Clear();
					messageDataList.Add(Convert.ToString(reader[0]));
					messageDataList.Add(Convert.ToString(reader[1]));
					messageDataList.Add(Convert.ToString(reader[2]));
					messageDataList.Add(Convert.ToString(reader[3]));                    
					messageDataList.Add(Convert.ToString(reader[4]));					
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
				getMessagestopic.CommandText = "SELECT userName FROM topics where topicName = @topicName"; // the command
				getMessagestopic.Parameters.AddWithValue("@topicName", name2);

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

		public (Dictionary<string, int>, List<string>) GetTopTopics()
		{
			string privacyOption = "public";
			List<string> userContactList = new List<string>();
			List<string> topicsList = new List<string>();
			List<string> categoryList = new List<string>();

			Dictionary<string, int> topicDictionary = new Dictionary<string, int>();
			Dictionary<string, int> sortedDict; 

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




				for (int counter = 0; counter < topicsList.Count(); counter++)
				{
					MySqlCommand getNumMessages = conn.CreateCommand();
					getNumMessages.CommandText = "SELECT count(*) FROM messagetopicbase where topicName= @topicName"; // the command
					getNumMessages.Parameters.AddWithValue("@topicName", topicsList[counter]);

					int numMessages = Convert.ToInt32(getNumMessages.ExecuteScalar());
					try
					{
						topicDictionary.Add(topicsList[counter], numMessages);
					}
					catch (ArgumentException e)
                    {

                    }
				}

					/*HashSet<string> knownValues = new HashSet<string>();
					Dictionary<string, int> uniqueValues = new Dictionary<string, int>();

					foreach (var pair in topicDictionary)
					{
						if (knownValues.Add(pair.Key))
						{
							uniqueValues.Add(pair.Key, pair.Value);
						}
					}*/

					sortedDict = (from entry in topicDictionary orderby entry.Value descending select entry).Take(3).ToDictionary(pair => pair.Key, pair => pair.Value);


				foreach (KeyValuePair<string, int> topic in sortedDict)
				{
					MySqlCommand getCategory = conn.CreateCommand();
					getCategory.CommandText = "SELECT category FROM topics where topicName = @topicName2"; // the command
					getCategory.Parameters.AddWithValue("@topicName2", topic.Key);

					// Execute the SQL command against the DB:
					MySqlDataReader reader2 = getCategory.ExecuteReader();
					int counter = 0;
					while (reader2.Read())
					{
						if (counter== 0)
                        {
							categoryList.Add(Convert.ToString(reader2[0]));
						}
						else
                        {
							break;
                        }
						counter++;
					}
					reader2.Close();
				}
				conn.Close();
			}
			return (sortedDict, categoryList);
		}

		public Dictionary<string, int> GetTopGroups()
		{
		//	string privacyOption = "public";
			List<string> userContactList = new List<string>();
			List<string> groupsList = new List<string>();
			Dictionary<string, int> groupDictionary = new Dictionary<string, int>();
			int i = 0;

			const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
			using (MySqlConnection conn = new MySqlConnection(connectionstring))
			{
				conn.Open();

				MySqlCommand getGroups = conn.CreateCommand();
				getGroups.CommandText = "SELECT chatName FROM groupmessage where userId = @userID";
				getGroups.Parameters.AddWithValue("@userID", DBObject.m_id);

				// Execute the SQL command against the DB:
				MySqlDataReader reader = getGroups.ExecuteReader();
				while (reader.Read())
				{
					if (i != 0)
					{
						if (Convert.ToString(reader[0]) == groupsList[i - 1])
						{
						}
						else
						{
							groupsList.Add(Convert.ToString(reader[0]));
							i++;
						}
					}
					else
					{
						groupsList.Add(Convert.ToString(reader[0]));
						i++;
					}
				}
				reader.Close();




				for (int counter = 0; counter < groupsList.Count(); counter++)
				{
					MySqlCommand getNumMessages = conn.CreateCommand();
					getNumMessages.CommandText = "SELECT count(*) FROM groupmessagetext where chatName= @groupName"; // the command
					getNumMessages.Parameters.AddWithValue("@groupName", groupsList[counter]);

					int numMessages = Convert.ToInt32(getNumMessages.ExecuteScalar());
					groupDictionary.Add(groupsList[counter], numMessages);
				}

				conn.Close();
			}

			Dictionary<string, int> sortedDict = (from entry in groupDictionary orderby entry.Value descending select entry).Take(3).ToDictionary(pair => pair.Key, pair => pair.Value);
			return sortedDict;
		}


	}
}
