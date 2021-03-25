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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult TopicSearch()
        {
            return View();
        }
        public IActionResult CreateTopic()
        {
            return View();
        }

        public IActionResult CreateGroupScreen()
        {
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
                    return RedirectToAction("Home", "Home");
                }
            }
            
            return View("CreateGroup");
            
        }

        public IActionResult GroupTemplate(GroupTemplateModel groupTemplateMod, string? name)
        {
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
    }
}
