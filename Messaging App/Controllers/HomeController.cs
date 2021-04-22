using MessagingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MessagingApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Home");
        }
        public IActionResult Home(HomeModel homeMod, Preferences pc)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand getGroups = conn.CreateCommand();
            getGroups.CommandText = "SELECT chatName FROM groupmessage where userID = @userID and Invite = @Invite"; // the command
            getGroups.Parameters.AddWithValue("@userID", DBObject.m_id);
            getGroups.Parameters.AddWithValue("@Invite", false);

            MySqlDataReader reader = getGroups.ExecuteReader();

            List<string> groupsList = new List<string>();

            while (reader.Read())
            {
                groupsList.Add(Convert.ToString(reader[0]));
            }
            reader.Close();

            homeMod.SetGroupListAttr(groupsList);
     
            MySqlCommand gettopics = conn.CreateCommand();
            gettopics.CommandText = "SELECT topicName FROM topics where userid= @userID"; // the command
            gettopics.Parameters.AddWithValue("@userID", DBObject.m_id);

            MySqlDataReader reader1 = gettopics.ExecuteReader();

            List<string> topicList = new List<string>();

            while (reader1.Read())
            {
                topicList.Add(Convert.ToString(reader1[0]));
            }
            reader1.Close();

            homeMod.SetttopicListAttr(topicList);      

            // set background color and textcolor that user selected
            MySqlCommand getContacts = conn.CreateCommand();
            getContacts.CommandText = "SELECT backgroundColor, fontColor FROM preferences where id = @userID"; // the command
            getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader reader2 = getContacts.ExecuteReader();

            List<string> txtBackgroundColorList = new List<string>();

            int i = 0;
            while (i != 2)
            {
                reader2.Read();
                if (reader2.HasRows == false)
                {
                    break;
                }
                txtBackgroundColorList.Add(Convert.ToString(reader2[i]));
                i++;
                    
            }

            reader2.Close();
            if (txtBackgroundColorList.Count() == 0)
            {
                pc.TxtColor = "black";
                pc.Baccolor = "url(../../Images/EarthBackground.jpeg)";

                string txtcmd = $"Insert into preferences (id,fontColor,backgroundColor)" + $"values ( @id, @TextColor,@BackgroundColor) ";
                MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id", DBObject.m_id);
                cmd.Parameters.AddWithValue("@TextColor", pc.TxtColor);
                cmd.Parameters.AddWithValue("@BackgroundColor", pc.Baccolor);
                cmd.Prepare();
                cmd.ExecuteReader();
                conn.Close();
            }
            
            else
            {
                DBObject.Bcolor = txtBackgroundColorList[0];
                DBObject.Tcolor = txtBackgroundColorList[1];
            }
            conn.Close();

            // set user contacts to home model contact list for home screen
            DBManager _manager = new DBManager();
            List<string> contacts = new List<string>();

            contacts = _manager.GetUserContacts();

            homeMod.SetContactListAttr(contacts);


            Dictionary<string, int> groupDictionary = new Dictionary<string, int>();
            groupDictionary = _manager.GetTopGroups();

            ViewData["groupCount"] = groupDictionary;



            Dictionary<string, int> topicDictionary = new Dictionary<string, int>();
            List<string> categoryList = new List<string>();
            (topicDictionary, categoryList) = _manager.GetTopTopics();

            ViewData["topicCount"] = topicDictionary;
            ViewData["topicCategory"] = categoryList;


            MySqlCommand getgroups = conn.CreateCommand();
            getgroups.CommandText = "SELECT chatName,Invite,Sender FROM groupmessage where userId= @userId";
            getgroups.Parameters.AddWithValue("@userId", DBObject.m_id);
            conn.Open();
            MySqlDataReader lRead = getgroups.ExecuteReader();
            List<string> groupinviteList = new List<string>();
            List<int> InviteList = new List<int>();
            List<int> groupSenderList = new List<int>();
            while (lRead.Read())
            {
               groupinviteList.Add(Convert.ToString(lRead[0]));
               InviteList.Add(Convert.ToInt32(lRead[1]));
               groupSenderList.Add(Convert.ToInt32(lRead[2]));
            }
            lRead.Close();
            conn.Close();
           
            homeMod.SetinviteListAttr(InviteList);


            homeMod.SetgroupSenderListAttr(groupSenderList);

            homeMod.SetGroupinviteListAttr(groupinviteList);


            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult EditContacts(string username)
        {
            int id = 0;
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            MySqlCommand cmd2 = new MySqlCommand(null, conn);
            cmd2.CommandText = $"SELECT id FROM users where username='" + username + "'"; // the command
            MySqlDataReader cRead;
            conn.Open();

            using (cRead = cmd2.ExecuteReader()) // executes the search command
            {
                if (cRead.Read())
                {
                    id = Convert.ToInt32(cRead.GetValue(0));

                }
            }
            cRead.Close();
            conn.Close();

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(null, conn);
            cmd.CommandText = "update contacts SET FriendRequest ='" + 1 + "' Where UserId ='" + DBObject.m_id + "' and id_newContact='" + id + "' and username ='" + DBObject.m_username + "' and username_newContact ='" + username + "'";
            cmd.Prepare();
            cmd.ExecuteReader();
            conn.Close();

            conn.Open();
            MySqlCommand cmd3 = new MySqlCommand(null, conn);
            cmd3.CommandText = "update contacts SET FriendRequest ='" + 1 + "' Where UserId ='" + id + "' and id_newContact='" + DBObject.m_id + "' and username ='" + username + "' and username_newContact ='" + DBObject.m_username + "'";
            cmd3.Prepare();
            cmd3.ExecuteReader();
            conn.Close();


            return RedirectToAction("Contacts");

        }
        public IActionResult Contacts(HomeModel homeMod, ContactsModel cm)
        {
            string username = cm.addContactInput;
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            string txtcmd2 = $"SELECT id_newContact FROM contacts where UserId='" + DBObject.m_id + "' and username_newContact = '" + username + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
            MySqlDataReader dRead;
            using (dRead = cmd2.ExecuteReader()) // executes the search command
            {
                if (dRead.Read() && username != null)
                {
                    conn.Close();
                    dRead.Close();
                    ViewBag.Message = "User is already contact";
                    return View("Contacts");

                }
                else
                {
                    conn.Close();
                    dRead.Close();
                }
            }

            txtcmd2 = $"SELECT id FROM users where username='" + username + "'"; // the command
            cmd2 = new MySqlCommand(txtcmd2, conn);
            MySqlDataReader cRead;
            conn.Open();
            if (username != null)
            {
                using (cRead = cmd2.ExecuteReader()) // executes the search command
                {
                    if (cRead.Read() && DBObject.m_username != username && username != null)
                    {
                        int id = Convert.ToInt32(cRead.GetValue(0));

                        MySqlCommand cmd = new MySqlCommand(null, conn);
                        cmd.CommandText = $"Insert into contacts (UserId,id_newContact,username, username_newContact, FriendRequest,Sender)" + $"values ( @UserId, @id_newContact,@username,@username_newContact, @FriendRequest,@Sender) ";
                        cmd.Parameters.AddWithValue("@UserId", DBObject.m_id);
                        cmd.Parameters.AddWithValue("@id_newContact", id);
                        cmd.Parameters.AddWithValue("@username", DBObject.m_username);
                        cmd.Parameters.AddWithValue("@username_newContact", username);
                        cmd.Parameters.AddWithValue("@FriendRequest", 0);
                        cmd.Parameters.AddWithValue("@Sender", 1);
                        cRead.Close();
                        cmd.Prepare();
                        cmd.ExecuteReader();
                        conn.Close();

                        conn.Open();
                        MySqlCommand cmd4 = new MySqlCommand(null, conn);
                        cmd4.CommandText = $"Insert into contacts (UserId,id_newContact,username, username_newContact, FriendRequest, Sender)" + $"values ( @UserId, @id_newContact,@username,@username_newContact, @FriendRequest,@Sender) ";
                        cmd4.Parameters.AddWithValue("@UserId", id);
                        cmd4.Parameters.AddWithValue("@id_newContact", DBObject.m_id);
                        cmd4.Parameters.AddWithValue("@username", username);
                        cmd4.Parameters.AddWithValue("@username_newContact", DBObject.m_username);
                        cmd4.Parameters.AddWithValue("@FriendRequest", 0);
                        cmd4.Parameters.AddWithValue("@Sender", 0);
                        cmd4.Prepare();
                        cmd4.ExecuteReader();

                        conn.Close();
                    }
                    else
                    {
                        ViewBag.Message = "Not a valid User";
                        cRead.Close();
                        conn.Close();
                    }
                }
            }
            else
            {
                conn.Close();
            }
            MySqlCommand getContacts = conn.CreateCommand();
            getContacts.CommandText = "SELECT username_newContact,FriendRequest,Sender FROM contacts where userID= @userID"; // the command
            getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);
            conn.Open();
            MySqlDataReader lRead = getContacts.ExecuteReader();
            List<string> ContactsList = new List<string>();
            List<int> ContactnumList = new List<int>();
            List<int> ContactSenderList = new List<int>();
            while (lRead.Read())
            {
                ContactsList.Add(Convert.ToString(lRead[0]));
                ContactnumList.Add(Convert.ToInt32(lRead[1]));
                ContactSenderList.Add(Convert.ToInt32(lRead[2]));
            }
            lRead.Close();
            conn.Close();

            
            homeMod.SetContactnumberListAttr(ContactnumList);

   
            homeMod.SetContactSenderListAttr(ContactSenderList);

            homeMod.SetcontactsListAttr(ContactsList);

            return View("Contacts");
        }

        public IActionResult removeContact(string contact)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand removecontact = conn.CreateCommand();
            removecontact.CommandText = "Delete FROM contacts where userID= @userID AND username_newContact = @Contact"; // the command
            removecontact.Parameters.AddWithValue("@userID", DBObject.m_id);
            removecontact.Parameters.AddWithValue("@Contact", contact);
            removecontact.Prepare();
            removecontact.ExecuteReader();
            conn.Close();

            conn.Open();
            MySqlCommand removecontact2 = conn.CreateCommand();
            removecontact2.CommandText = "Delete FROM contacts where userName= @userName AND username_newContact = @Contact"; // the command
            removecontact2.Parameters.AddWithValue("@userName", contact);
            removecontact2.Parameters.AddWithValue("@Contact", DBObject.m_username);
            removecontact2.Prepare();
            removecontact2.ExecuteReader();
            conn.Close();

            return RedirectToAction("Contacts", "Home");
        }


        public IActionResult RemovePinnedMessagesgroup(string message, string user, string chatname, string image)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            if (message != null && image != null)
            {
                string txtcmd1 = "delete FROM pinnedMessages where (userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicgroupName = @topicGroupName and image = @image)";
                MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                cmd1.Parameters.AddWithValue("@userName", user);
                cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                cmd1.Parameters.AddWithValue("@topicGroupName", chatname);
                cmd1.Parameters.AddWithValue("@image", image);
                cmd1.ExecuteNonQuery();
                conn.Close();

            }
            else if (message == null && image != null)
            {
                string txtcmd1 = "delete FROM pinnedMessages where (userId = @userID and userName = @userName and topicgroupName = @topicGroupName and image = @image)";
                MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                cmd1.Parameters.AddWithValue("@userName", user);
                cmd1.Parameters.AddWithValue("@topicGroupName", chatname);
                cmd1.Parameters.AddWithValue("@image", image);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }
            else if (message != null && image == null)
            {
                string txtcmd1 = "delete FROM pinnedMessages where (userId = @userID and userName = @userName and pinnedMessages = @pinnedMessage and topicGroupName = @topicGroupName)";
                MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                cmd1.CommandType = CommandType.Text;
                cmd1.Parameters.AddWithValue("@userID", DBObject.m_id);
                cmd1.Parameters.AddWithValue("@userName", user);
                cmd1.Parameters.AddWithValue("@pinnedMessage", message);
                cmd1.Parameters.AddWithValue("@topicGroupName", chatname);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }


            return RedirectToAction("PinnedMessages", "Home");
        }
        public IActionResult PinnedMessages(HomeModel homeMod)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand getPinned = conn.CreateCommand();
            getPinned.CommandText = "SELECT pinnedMessages FROM pinnedMessages where userId= @userID"; // the command
            getPinned.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader aRead = getPinned.ExecuteReader();
            List<string> PinnedList = new List<string>();

            while (aRead.Read())
            {
                PinnedList.Add(Convert.ToString(aRead[0]));
            }
            aRead.Close();

            MySqlCommand getuserPinned = conn.CreateCommand();
            getuserPinned.CommandText = "SELECT userName FROM pinnedMessages where userId= @userID"; // the command
            getuserPinned.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader bRead = getuserPinned.ExecuteReader();
            List<string> userPinnedList = new List<string>();

            while (bRead.Read())
            {
                userPinnedList.Add(Convert.ToString(bRead[0]));
            }
            bRead.Close();

            MySqlCommand getgroupPinned = conn.CreateCommand();
            getgroupPinned.CommandText = "SELECT topicgroupName FROM pinnedMessages where userId= @userID"; // the command
            getgroupPinned.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader cRead = getgroupPinned.ExecuteReader();
            List<string> groupPinnedList = new List<string>();

            while (cRead.Read())
            {
                groupPinnedList.Add(Convert.ToString(cRead[0]));
            }
            cRead.Close();

            MySqlCommand getImagepinned = conn.CreateCommand();
            getImagepinned.CommandText = "SELECT image FROM pinnedMessages where userId= @userID"; // the command
            getImagepinned.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader dRead = getImagepinned.ExecuteReader();
            List<string> imagePinnedlist = new List<string>();

            while (dRead.Read())
            {
                imagePinnedlist.Add(Convert.ToString(dRead[0]));
            }
            dRead.Close();

            MySqlCommand getMessageType = conn.CreateCommand();
            getMessageType.CommandText = "SELECT messageType FROM pinnedMessages where userId= @userID"; // the command
            getMessageType.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader eRead = getMessageType.ExecuteReader();
            List<string> messageTypeList = new List<string>();

            while (eRead.Read())
            {
                messageTypeList.Add(Convert.ToString(eRead[0]));
            }
            dRead.Close();

            homeMod.SetPinnedListAttr(PinnedList);
            homeMod.SetgroupPinnedListAttr(groupPinnedList);
            homeMod.SetuserPinnedListAttr(userPinnedList);
            homeMod.SetimagePinnedListAttr(imagePinnedlist);
            homeMod.SetmessageTypePinnedListAttr(messageTypeList);

            return View();
        }
    

         public IActionResult PreferencesScreen()
        {
            return View("Preferences");
        }

        public IActionResult Preferences(Preferences pc)
        {
            string BackgroundColor = pc.Baccolor;
            string TextColor = pc.TxtColor;

            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            string txtcmd2 = $"SELECT id FROM preferences where id='" + DBObject.m_id + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
            MySqlDataReader dRead;
            using (dRead = cmd2.ExecuteReader()) // executes the search command
            {
                if (dRead.Read())
                {
                    conn.Close();
                    string txtcmd = "update preferences SET backgroundColor ='" + BackgroundColor + "', fontColor ='" + TextColor + "' Where id ='" + DBObject.m_id + "'"; // the command
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    conn.Open();
                    cmd.Prepare();
                    cmd.ExecuteReader();
                    ViewBag.message = ("Updated Preferences");
                }
                else
                {
                    conn.Close();
                    string txtcmd = $"Insert into preferences (id,fontColor,backgroundColor)" + $"values ( @id, @TextColor,@BackgroundColor) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", DBObject.m_id);
                    cmd.Parameters.AddWithValue("@TextColor", TextColor);
                    cmd.Parameters.AddWithValue("@BackgroundColor", BackgroundColor);
                    cmd.Prepare();
                    cmd.ExecuteReader();
                }
            }
            dRead.Close();
            conn.Close();

            DBObject.Tcolor = TextColor;
            DBObject.Bcolor = BackgroundColor;
            return View("Preferences");
        }

        public IActionResult AccountSettings(AccountSettings ac)
        {
            var NAS = new AccountSettings
            {
                userName = DBObject.m_username,
                Email = DBObject.m_email
            };

            string Email = ac.Email;
            if (Email != null)
            {
                const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                string txtcmd = "update users SET email ='" + Email + "' Where id ='" + DBObject.m_id + "'"; // the command

                MySqlConnection conn = new MySqlConnection(connectionstring);
                MySqlCommand cmd = new MySqlCommand(txtcmd, conn);

                conn.Open();
                cmd.ExecuteNonQuery();

                ViewBag.message = ("Email Changed");
                DBObject.m_email = Email;
                conn.Close();
            }
            string Username = ac.userName;
            if (Username != null)
            {
                const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                MySqlConnection conn = new MySqlConnection(connectionstring);
                conn.Open();
                string txtcmd2 = $"SELECT id FROM users where username='" + Username + "'"; // the command
                MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
                MySqlDataReader dRead;
                using (dRead = cmd2.ExecuteReader()) // executes the search command
                {
                    if (dRead.Read())
                    {
                        conn.Close();
                        ViewBag.message = "UserName Already Taken";
                        return View("AccountSettings");

                    }
                }
                dRead.Close();
                conn.Close();

                string txtcmd = "update users SET username ='" + Username + "' Where id ='" + DBObject.m_id + "'"; // the command
                MySqlCommand cmd = new MySqlCommand(txtcmd, conn);

                conn.Open();
                cmd.ExecuteNonQuery();

                ViewBag.message = ("Username Changed");
                DBObject.m_username = Username;
                conn.Close();
            }
            return View(NAS);
        }


        public IActionResult savePicture()
        {
            byte[] imageFile;
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            string txtcmd2 = $"SELECT id FROM preferences where id='" + DBObject.m_id + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
            MySqlDataReader dRead;

            using (dRead = cmd2.ExecuteReader()) // executes the search command
            {
                if (dRead.Read())
                {
                    imageFile = (Byte[])(dRead["image"]);
                    /*
                     There's a error here when the choose file button gets pressed with the line above
                     */
                    conn.Close();
                    string txtcmd = "update preferences SET picture ='" + imageFile + "' Where id ='" + DBObject.m_id + "'"; // the command
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    conn.Open();
                    cmd.Prepare();
                    cmd.ExecuteReader();
                    ViewBag.message = ("Profile Picture Changed");
                }
                else
                {
                    imageFile = (Byte[])(dRead["image"]);
                    /*
                    There's a error here when the choose file button gets pressed with the line above
                    */
                    conn.Close();
                    string txtcmd = $"Insert into preferences (id,picture)" + $"values ( @id, @pictureFile) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    conn.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@id", DBObject.m_id);
                    cmd.Parameters.AddWithValue("@pictureFile", imageFile);
                    cmd.Prepare();
                    cmd.ExecuteReader();
                }
            }
            dRead.Close();
            conn.Close();
            return View("");
        }

        public IActionResult ChangePasswordScreen()
        {
            return View("ChangePassword");
        }

        public IActionResult ChangePassword(ChangePassword cp)
        {
            string Password1 = cp.NewPassword;
            string ConPassword1 = cp.ConfirmPassword;

            if (Password1 == null || ConPassword1 != Password1)
            {
                return View();
            }
            else
            {
                const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                string txtcmd = "update users SET password ='" + Password1 + "' Where id ='" + DBObject.m_id + "'"; // the command

                MySqlConnection conn = new MySqlConnection(connectionstring);
                MySqlCommand cmd = new MySqlCommand(txtcmd, conn);

                conn.Open();
                cmd.ExecuteNonQuery();

                ViewBag.message = (TempData["Password Changed"]="Password has been changed");

                conn.Close();
                return RedirectToAction("AccountSettings", "Home");                 
            }
        }



        public IActionResult EditgroupInvite(string chatName)
        {
            int id = 0;
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand(null, conn);
            cmd.CommandText = "update groupmessage SET Invite ='" + 0 + "' Where UserId ='" + DBObject.m_id + "' and chatName ='" + chatName + "'";
            cmd.Prepare();
            cmd.ExecuteReader();
            conn.Close();

            return RedirectToAction("Home");

        }

        public IActionResult removeGroupInvite(string chatName)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand removeGroup = conn.CreateCommand();
            removeGroup.CommandText = "Delete FROM groupmessage where userID= @userID AND chatName = @chatName"; // the command
            removeGroup.Parameters.AddWithValue("@userID", DBObject.m_id);
            removeGroup.Parameters.AddWithValue("@chatName", chatName);
            removeGroup.Prepare();
            removeGroup.ExecuteReader();
            conn.Close();

            return RedirectToAction("Home");
        }

    }
}
