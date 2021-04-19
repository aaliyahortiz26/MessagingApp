using MessagingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.SignalR;                                         

namespace MessagingApp.Controllers
{
    public class ChatController : Controller
    {
        /*private readonly DBManager _manager;

        public ChatController(DBManager manager)
        {
            _manager = manager;
        }*/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TopicSearch(TopicSearchModel topicSearchMod)
        {
            string privateOption = "public";

            const string connection2 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            const string connection3 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            
            MySqlConnection conn2 = new MySqlConnection(connection2);
            MySqlConnection conn3 = new MySqlConnection(connection3);
            MySqlCommand topicSearch = conn2.CreateCommand();
            MySqlCommand topicCategory = conn3.CreateCommand();
            
            topicCategory.CommandText = "SELECT category FROM topics where privacyOption = @privacyOption"; //command for category
            topicCategory.Parameters.AddWithValue("privacyOption", privateOption);
            conn3.Open();

            MySqlDataReader mRead = topicCategory.ExecuteReader();
            List<string> CategorySearch = new List<string>();

            int i = 0;
            bool categoryExist = false;
            while (mRead.Read())
            {
                if (i != 0)
                {
                    for (int counter = 0; counter < CategorySearch.Count(); counter++)
                    {
                        if (Convert.ToString(mRead[0]) == CategorySearch[counter])
                        {
                            categoryExist = true;
                        }
                    }
                    if(categoryExist == false)
                    {
                        CategorySearch.Add(Convert.ToString(mRead[0]));
                        i++;
                    }
                }
                else
                {
                    CategorySearch.Add(Convert.ToString(mRead[0]));
                    i++;
                }
                categoryExist = false;
            }
            mRead.Close();
            // conn3.Close();

            topicSearch.CommandText = "SELECT topicName FROM topics where privacyOption = @privacyOption";
            topicSearch.Parameters.AddWithValue("@privacyOption", privateOption);

            conn2.Open();
            MySqlDataReader lRead = topicSearch.ExecuteReader();
            List<string> TopicSearch = new List<string>();

            while (lRead.Read())
            {
                TopicSearch.Add(Convert.ToString(lRead[0]));
            }
            lRead.Close();
            conn2.Close();

            topicSearchMod.SetCategoryListAttr(CategorySearch);
            topicSearchMod.SetTopicsListAttr(TopicSearch);
            return View("TopicSearch");
        }
        public IActionResult GetTopicList(TopicSearchModel TopSerMod, string category)
        {
            /**
             
             WAS TRYING SOMETHING OUT HERE
             */
            string privateOption = "public";

            const string connection7 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn7 = new MySqlConnection(connection7);
            MySqlCommand topicSearch = conn7.CreateCommand();

            if(category == "")
            {
                topicSearch.CommandText = "SELECT topicName FROM topics where privacyOption = @privacyOption";
                topicSearch.Parameters.AddWithValue("@privacyOption", privateOption);
            } 
            else
            {
                topicSearch.CommandText = "SELECT topicName FROM topics where privacyOption = @privacyOption AND category = @category";
                topicSearch.Parameters.AddWithValue("@privacyOption", privateOption);
                topicSearch.Parameters.AddWithValue("@category", category);
            }

            conn7.Open();
            MySqlDataReader lRead = topicSearch.ExecuteReader();
            List<string> TopicSearch = new List<string>();

            while (lRead.Read())
            {
                TopicSearch.Add(Convert.ToString(lRead[0]));
            }
            lRead.Close();
            conn7.Close();

            TopSerMod.SetTopicsListAttr(TopicSearch);
            return View("GetTopicList");
        }
            public IActionResult ViewTopic(TopicSearchModel tSM)
        {
            const string connection4 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn4 = new MySqlConnection(connection4);
            MySqlCommand viewTopic = conn4.CreateCommand();
            tSM.m_topic = tSM.topicDropdown;
            tSM.m_category = tSM.categoryDropdown;

            viewTopic.CommandText = "SELECT description, topicQuestion FROM topics where category= @category AND topicName= @topicName"; // the command
            viewTopic.Parameters.AddWithValue("@category", tSM.m_category);
            viewTopic.Parameters.AddWithValue("@topicName", tSM.m_topic);
            conn4.Open();
            MySqlDataReader vRead = viewTopic.ExecuteReader();

            while (vRead.Read())
            {
                tSM.m_description = vRead[0].ToString();
                tSM.m_question = vRead[1].ToString();
            }

            vRead.Close();
            conn4.Close();

            return View("ViewTopic");
        }

        public IActionResult JoinTopic(TopicSearchModel topicSearchM)
        {
            const string connection6 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn6 = new MySqlConnection(connection6);

            //string txtCmd = $"SELECT (privacyOption, username, contactName) FROM topics WHERE topicName= @topicName2";
            MySqlCommand joinTopic = conn6.CreateCommand();
            joinTopic.CommandText = $"SELECT privacyOption, contactName FROM topics WHERE topicName= @topicName2";
            joinTopic.Parameters.AddWithValue("@topicName2", topicSearchM.m_topic);
            conn6.Open();
            MySqlDataReader jRead = joinTopic.ExecuteReader();

            while (jRead.Read())
            {
                topicSearchM.radioField = jRead[0].ToString();
                topicSearchM.m_contactName = jRead[1].ToString();
            }
            jRead.Close();

            int userIdTopic = 0;
            string txtCmd3 = $"SELECT id FROM users where username='" + topicSearchM.m_contactName + "'"; // the command
            MySqlCommand cmd3 = new MySqlCommand(txtCmd3, conn6);
            MySqlDataReader hRead;
            using (hRead = cmd3.ExecuteReader()) // executes the search command
            {
                if (hRead.Read())
                {
                    userIdTopic = Convert.ToInt32(hRead[0]);
                }
            }
            hRead.Close();

            //for adding user on one end
            string txtCmd2 = $"Insert into united_messaging.topics (userid, topicName, category, description,privacyOption, topicQuestion, contactName, userName)" + $"values ( @userID3, @topicName3, @category3, @description3, @privacyOption3, @topicQuestion3, @contactName3, @userName3)";
            MySqlCommand joinTop = new MySqlCommand(txtCmd2, conn6);
            joinTop.CommandType = CommandType.Text;
            joinTop.Parameters.AddWithValue("@userID3", DBObject.m_id);
            joinTop.Parameters.AddWithValue("@topicName3", topicSearchM.m_topic);
            joinTop.Parameters.AddWithValue("@category3", topicSearchM.m_category);
            joinTop.Parameters.AddWithValue("@description3", topicSearchM.m_description);
            joinTop.Parameters.AddWithValue("@privacyOption3", topicSearchM.radioField);
            joinTop.Parameters.AddWithValue("@topicQuestion3", topicSearchM.m_question);
            joinTop.Parameters.AddWithValue("@contactName3", topicSearchM.m_contactName);
            joinTop.Parameters.AddWithValue("@userName3", DBObject.m_username);
            joinTop.ExecuteNonQuery();

            conn6.Close();
            return RedirectToAction("Home", "Home");
        }
        public IActionResult CreateTopicScreen()
        {
            List<string> ContactsList = new List<string>();
            DBManager _manager = new DBManager();

            ContactsList = _manager.GetUserContacts();

            ViewData["userContacts"] = ContactsList;

            return View("CreateTopic");
        }
        public IActionResult CreateTopic(CreateTopicModel ctm)
        {
            if (ModelState.IsValid)
            {
                // fill dropdown with user's contact names
                List<string> ContactsList = new List<string>();
                DBManager _manager = new DBManager();

                ContactsList = _manager.GetUserContacts();

                ViewData["userContacts"] = ContactsList;

                // check to see if topic exists
                const string connection1 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                MySqlConnection conn1 = new MySqlConnection(connection1);

                conn1.Open();

                MySqlCommand getGroups = conn1.CreateCommand();
                getGroups.CommandText = "SELECT count(*) FROM topics where topicName= @chatName"; // the command
                getGroups.Parameters.AddWithValue("@chatName", ctm.topicName);

                int groupExist = Convert.ToInt32(getGroups.ExecuteScalar());
                conn1.Close();

                if (groupExist >= 1)
                {
                    ViewBag.message = "Topic already exists";
                    return View("CreateTopic");
                    //return RedirectToAction("CreateTopicScreen", "Chat");
                }
                else if (ctm.radioField == null)
                {
                    ViewBag.message = "Please choose Private or Public";
                    return View("CreateTopic");
                    //return RedirectToAction("CreateTopicScreen", "Chat");

                }
                else
                {
                    const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                    MySqlConnection conn = new MySqlConnection(connectionstring);

                    conn.Open();

                    string txtcmd = $"Insert into united_messaging.topics (userid, topicName, category, description,privacyOption, topicQuestion, contactName, userName)" + $"values ( @userID, @topicName, @category, @description, @privacyOption, @topicQuestion, @contactName, @userName)";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd.Parameters.AddWithValue("@topicName", ctm.topicName);
                    cmd.Parameters.AddWithValue("@category", ctm.category);
                    cmd.Parameters.AddWithValue("@description", ctm.description);
                    cmd.Parameters.AddWithValue("@privacyOption", ctm.radioField);
                    cmd.Parameters.AddWithValue("@topicQuestion", ctm.question);
                    cmd.Parameters.AddWithValue("@contactName", ctm.inviteContact);
                    cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
                    cmd.ExecuteNonQuery();
                    conn.Close();


                    const string connectionstring2 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                    MySqlConnection conn2 = new MySqlConnection(connectionstring2);

                    conn2.Open();
                    int contactID = 0;
                    string txtcmd1 = $"SELECT id FROM users where username='" + ctm.inviteContact + "'"; // the command
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn2);
                    MySqlDataReader dRead;
                    using (dRead = cmd1.ExecuteReader()) // executes the search command
                    {
                        if (dRead.Read())
                        {
                            contactID = Convert.ToInt32(dRead[0]);
                        }
                    }
                    dRead.Close();

                    string txtcmd2 = $"Insert into united_messaging.topics (userid, topicName, category, description,privacyOption, topicQuestion, contactName, userName)" + $"values ( @userID2, @topicName2, @category2, @description2, @privacyOption2, @topicQuestion2, @contactName2, @userName2)";
                    MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn2);
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Parameters.AddWithValue("@userID2", contactID);
                    cmd2.Parameters.AddWithValue("@topicName2", ctm.topicName);
                    cmd2.Parameters.AddWithValue("@category2", ctm.category);
                    cmd2.Parameters.AddWithValue("@description2", ctm.description);
                    cmd2.Parameters.AddWithValue("@privacyOption2", ctm.radioField);
                    cmd2.Parameters.AddWithValue("@topicQuestion2", ctm.question);
                    cmd2.Parameters.AddWithValue("@contactName2", DBObject.m_username);
                    cmd2.Parameters.AddWithValue("@userName2", ctm.inviteContact);
                    cmd2.ExecuteNonQuery();
                    conn2.Close();
                }
                return RedirectToAction("Home", "Home");

            }

            return View("CreateTopic");

        }

        public IActionResult CreateGroupScreen(HomeModel homeMod)
        {
            const string connection2 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn2 = new MySqlConnection(connection2);
            MySqlCommand getContacts = conn2.CreateCommand();
            getContacts.CommandText = "SELECT username_newContact FROM contacts where userID= @userID"; // the command
            getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);
            conn2.Open();
            MySqlDataReader lRead = getContacts.ExecuteReader();
            List<string> ContactsList = new List<string>();

            while (lRead.Read())
            {
                ContactsList.Add(Convert.ToString(lRead[0]));
            }
            lRead.Close();

            homeMod.SetcontactsListAttr(ContactsList);
            conn2.Close();
            return View("CreateGroup");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateGroup (CreateGroupModel createGroupMod)
        {            
            if (ModelState.IsValid)
            {
                const string connection1 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                MySqlConnection conn1 = new MySqlConnection(connection1);

                conn1.Open();

                MySqlCommand getGroups = conn1.CreateCommand();
                getGroups.CommandText = "SELECT count(*) FROM groupmessage where chatName= @chatName"; // the command
                getGroups.Parameters.AddWithValue("@chatName", createGroupMod.groupChatTitle);

                int groupExist = Convert.ToInt32(getGroups.ExecuteScalar());

                conn1.Close();

                if (groupExist >= 1)
                {
                    ViewBag.message = "Group already exists";
                    return View("CreateGroup");
                }
                else if (createGroupMod.groupChatTitle == "")
                {
                    return View("CreateGroup");
                }                            
                else
                {
                    const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                    MySqlConnection conn = new MySqlConnection(connectionstring);

                    conn.Open();

                    string txtcmd = $"Insert into united_messaging.groupmessage (userid, chatName, contactName, userName)" + $"values ( @userID, @groupChatTitle,@inviteContact, @userName) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userID", createGroupMod.ID);
                    cmd.Parameters.AddWithValue("@groupChatTitle", createGroupMod.groupChatTitle);
                  //  cmd.Parameters.AddWithValue("@privacyOption", createGroupMod.radioField);
                    cmd.Parameters.AddWithValue("@inviteContact", createGroupMod.inviteContact);
                    cmd.Parameters.AddWithValue("@userName", DBObject.m_username);

                    cmd.ExecuteNonQuery();
                    conn.Close();

                    
                    const string connectionstring2 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                    MySqlConnection conn2 = new MySqlConnection(connectionstring2);

                    conn2.Open();
                    int contactID = 0;
                    string txtcmd1 = $"SELECT id FROM users where username='" + createGroupMod.inviteContact + "'"; // the command
                    MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn2);
                    MySqlDataReader dRead;
                    using (dRead = cmd1.ExecuteReader()) // executes the search command
                    {
                        if (dRead.Read())
                        {
                            contactID = Convert.ToInt32(dRead[0]);
                        }
                    }
                    dRead.Close();

                    string txtcmd2 = $"Insert into united_messaging.groupmessage (userid, chatName, contactName, userName)" + $"values ( @contactUserID, @groupChatContactTitle,@inviteUserContact, @userName) ";
                    MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn2);
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Parameters.AddWithValue("@contactUserID", contactID);
                    cmd2.Parameters.AddWithValue("@groupChatContactTitle", createGroupMod.groupChatTitle);
                //    cmd2.Parameters.AddWithValue("@privacyContactOption", createGroupMod.radioField);
                    cmd2.Parameters.AddWithValue("@inviteUserContact", DBObject.m_username);
                    cmd2.Parameters.AddWithValue("@userName", createGroupMod.inviteContact);

                    cmd2.ExecuteNonQuery();
                    conn2.Close();
                }
                return RedirectToAction("Home", "Home");
                
            }
            
            return View("CreateGroup");
            
        }

        public IActionResult GroupTemplate(GroupTemplateModel groupTemplateMod, string? name)
        {
            DBManager _manager = new DBManager();
            List<Messages> messages = new List<Messages>();
            List<string> users = new List<string>();

            messages = _manager.GetMessagesGroup(name);

            ViewData["messageobjects"] = messages;


            users = _manager.GetGroupUsers(name);
            ViewData["userobjects"] = users;

            // bool foundUsername = false;
            //string groupChatName = name;
            /*  for (int i = 0; i < HomeModel.m_grouplist.Count; i++)
              {
                  if (name == HomeModel.m_grouplist[i])
                  {
                      foundUsername = true;
                  }
              }*/
            /*if (name == "6")
            {
                return View();
            }
            else
            {
                return View();
            }*/

            /*   if (foundUsername == true)
               {
                   // select from database based on username of group and print out title
                   // 


               }*/
            groupTemplateMod.groupName = name;

            return View();
        }
        public IActionResult TopicTemplate(TopicTemplateModel topicTemplateMod, string? name)
        {
            DBManager _manager = new DBManager();
            List<string> discussionAndQuestion = new List<string>();
            List<Messages> messages = new List<Messages>();
            List<string> users = new List<string>();

            discussionAndQuestion = _manager.GetDiscussionQuestionTopic(name);
            ViewData["discussionAndQuestion"] = discussionAndQuestion;

            messages = _manager.GetMessagesTopic(name);
            ViewData["messageobjects"] = messages;

            users = _manager.GettopicUsers(name);
            ViewData["users"] = users;
            topicTemplateMod.topicName = name;

            return View();
        }

        public ActionResult removetopic()
        {

            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand getNumMessages = conn.CreateCommand();
            getNumMessages.CommandText = "SELECT count(*) FROM topics where topicName = @topicName2"; // the command
            getNumMessages.Parameters.AddWithValue("@topicName2", DBObject.m_TopicName);

            int numUsers = Convert.ToInt32(getNumMessages.ExecuteScalar());

            if (numUsers == 1)
            {
                MySqlCommand removeMessages = conn.CreateCommand();
                removeMessages.CommandText = "Delete FROM messagetopicbase where topicName = @topicName3"; // the command
                removeMessages.Parameters.AddWithValue("@userID", DBObject.m_id);
                removeMessages.Parameters.AddWithValue("@topicName3", DBObject.m_TopicName);
                removeMessages.ExecuteNonQuery();
            }

            MySqlCommand removetopic = conn.CreateCommand();
            removetopic.CommandText = "Delete FROM topics where userID= @userID AND topicName = @topicName"; // the command
            removetopic.Parameters.AddWithValue("@userID", DBObject.m_id);
            removetopic.Parameters.AddWithValue("@topicName", DBObject.m_TopicName);
            removetopic.Prepare();
            removetopic.ExecuteNonQuery();
            ViewBag.message = "Leaving Page";

            conn.Close();
            return RedirectToAction("Home", "Home");
        }

        public IActionResult removeGroup()
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();


            MySqlCommand getNumMessages = conn.CreateCommand();
            getNumMessages.CommandText = "SELECT count(*) FROM groupmessage where chatName = @chatName1"; // the command
            getNumMessages.Parameters.AddWithValue("@chatName1", DBObject.m_GroupName);

            int numUsers = Convert.ToInt32(getNumMessages.ExecuteScalar());

            if (numUsers == 1)
            {
                MySqlCommand removeMessages = conn.CreateCommand();
                removeMessages.CommandText = "Delete FROM groupmessagetext where chatName = @chatName2"; // the command
                removeMessages.Parameters.AddWithValue("@userID", DBObject.m_id);
                removeMessages.Parameters.AddWithValue("@chatName2", DBObject.m_GroupName);
                removeMessages.ExecuteNonQuery();
            }

            MySqlCommand removeGroup = conn.CreateCommand();
            removeGroup.CommandText = "Delete FROM groupmessage where userID= @userID AND chatName = @chatName"; // the command
            removeGroup.Parameters.AddWithValue("@userID", DBObject.m_id);
            removeGroup.Parameters.AddWithValue("@chatName", DBObject.m_GroupName);
            removeGroup.Prepare();
            removeGroup.ExecuteReader();
            conn.Close();
            ViewBag.message = "leaving Page";
            return RedirectToAction("Home", "Home");
        }
        public IActionResult PinnedMessagestopic(string message)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            string txtcmd1 = "select userName FROM pinnedMessages where userId = @userID and userName = @userName and topicgroupName = @topicgroupName";
            MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
            cmd1.Parameters.AddWithValue("@userName", DBObject.m_username);
            cmd1.Parameters.AddWithValue("@topicgroupName", DBObject.m_TopicName);
            MySqlDataReader mRead;
            using (mRead = cmd1.ExecuteReader()) // executes the search command
            {
                if (mRead.Read())
                {
                    conn.Close();
                    return RedirectToAction("TopicTemplate", new { name = DBObject.m_TopicName });

                }
                else
                {
                    mRead.Close();
                    string txtcmd = $"Insert into united_messaging.pinnedMessages (userid, userName,topicgroupName, pinnedMessages)" + $"values ( @userID, @userName,@topicgroupName,@pinnedMessages)";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
                    cmd.Parameters.AddWithValue("@topicgroupName", DBObject.m_TopicName);
                    cmd.Parameters.AddWithValue("@pinnedMessages", message);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return RedirectToAction("TopicTemplate", new { name = DBObject.m_TopicName });
                }
            }
        }
        public IActionResult PinnedMessagesgroup(string message)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            string txtcmd1 = "select userName FROM pinnedMessages where userId = @userID and userName = @userName and topicgroupName = @topicgroupName";
            MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
            cmd1.CommandType = CommandType.Text;
            cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
            cmd1.Parameters.AddWithValue("@userName", DBObject.m_username);
            cmd1.Parameters.AddWithValue("@topicgroupName", DBObject.m_GroupName);
            MySqlDataReader mRead;
            using (mRead = cmd1.ExecuteReader()) // executes the search command
            {
                if (mRead.Read())
                {
                    conn.Close();
                    return RedirectToAction("GroupTemplate", new { name = DBObject.m_GroupName });

                }
                else
                {
                    mRead.Close();
                    string txtcmd = $"Insert into united_messaging.pinnedMessages (userid, userName,topicgroupName, pinnedMessages)" + $"values ( @userID, @userName,@topicgroupName,@pinnedMessages)";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@userID", DBObject.m_id);
                    cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
                    cmd.Parameters.AddWithValue("@topicgroupName", DBObject.m_GroupName);
                    cmd.Parameters.AddWithValue("@pinnedMessages", message);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return RedirectToAction("GroupTemplate", new { name = DBObject.m_GroupName });
                }
            }
        }

        public IActionResult RemovePinnedMessagesgroup(string message)
        {

            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            string txtcmd = "Delete FROM pinnedMessages where userId = @userID and userName = @userName and topicgroupName = @topicgroupName and pinnedMessages = @pinnedMessages";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@userID", DBObject.m_id);
            cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
            cmd.Parameters.AddWithValue("@topicgroupName", DBObject.m_GroupName);
            cmd.Parameters.AddWithValue("@pinnedMessages", message);
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("GroupTemplate", new { name = DBObject.m_GroupName });
        }

        public IActionResult RemovePinnedMessagestopic(string message)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();
            string txtcmd = "Delete FROM pinnedMessages where userId = @userID and userName = @userName and topicgroupName = @topicgroupName and pinnedMessages = @pinnedMessages";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@userID", DBObject.m_id);
            cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
            cmd.Parameters.AddWithValue("@topicgroupName", DBObject.m_TopicName);
            cmd.Parameters.AddWithValue("@pinnedMessages", message);
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("TopicTemplate", new { name = DBObject.m_TopicName });

        }

        public ActionResult RemoveMessagesgroup(string message, string messageID)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();
            MySqlCommand deleteMessage = conn.CreateCommand();
            deleteMessage.CommandText = "Delete FROM groupmessagetext where chatName = @chatName and groupmessage = @groupMessage and Documentid = @documentID "; // the command
            deleteMessage.Parameters.AddWithValue("@chatName", DBObject.m_GroupName);
            deleteMessage.Parameters.AddWithValue("@groupMessage", message);
            deleteMessage.Parameters.AddWithValue("@documentID", messageID);

            deleteMessage.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("GroupTemplate", new {name = DBObject.m_GroupName });
        }

        public ActionResult RemoveMessagesTopic(string message, string messageID)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();
            MySqlCommand deleteMessage = conn.CreateCommand();
            deleteMessage.CommandText = "Delete FROM messagetopicbase where topicName = @topicName and topicMessage = @topicMessage and Documentid = @documentID "; // the command
            deleteMessage.Parameters.AddWithValue("@topicName", DBObject.m_TopicName);
            deleteMessage.Parameters.AddWithValue("@topicMessage", message);
            deleteMessage.Parameters.AddWithValue("@documentID", messageID);

            deleteMessage.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("TopicTemplate", new { name = DBObject.m_TopicName });
        }
    }
}