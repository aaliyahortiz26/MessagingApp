﻿using MessagingApp.Models;
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
        public IActionResult TopicSearch()
        {
            return View();
        }
        public IActionResult ViewTopic()
        {
            return View();
        }

        public IActionResult CreateTopic()
        {
            return View();
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

                    string txtcmd = $"Insert into united_messaging.groupmessage (userid, chatName, privacyOption, contactName)" + $"values ( @userID, @groupChatTitle,@privacyOption,@inviteContact) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userID", createGroupMod.ID);
                    cmd.Parameters.AddWithValue("@groupChatTitle", createGroupMod.groupChatTitle);
                    cmd.Parameters.AddWithValue("@privacyOption", createGroupMod.radioField);
                    cmd.Parameters.AddWithValue("@inviteContact", createGroupMod.inviteContact);
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

                    string txtcmd2 = $"Insert into united_messaging.groupmessage (userid, chatName, privacyOption, contactName)" + $"values ( @contactUserID, @groupChatContactTitle,@privacyContactOption,@inviteUserContact) ";
                    MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn2);
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Parameters.AddWithValue("@contactUserID", contactID);
                    cmd2.Parameters.AddWithValue("@groupChatContactTitle", createGroupMod.groupChatTitle);
                    cmd2.Parameters.AddWithValue("@privacyContactOption", createGroupMod.radioField);
                    cmd2.Parameters.AddWithValue("@inviteUserContact", DBObject.m_username);
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
        public IActionResult TopicTemplate(TopicTemplateModel topicTemplateMod,string? name)
        {
            topicTemplateMod.topicName = name;


            return View();
        }
    }
}
