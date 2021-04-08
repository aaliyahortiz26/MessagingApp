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
            getGroups.CommandText = "SELECT chatName FROM groupmessage where userID= @userID"; // the command
            getGroups.Parameters.AddWithValue("@userID", DBObject.m_id);

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

            reader.Close();
            if (txtBackgroundColorList.Count() == 0)
            {
                pc.TxtColor = "black";
                pc.Baccolor = "linear-gradient(blue, orange)";

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





			Dictionary<string, int> topicDictionary = new Dictionary<string, int>();
            topicDictionary = _manager.GetTopTopics();

            ViewData["topicCount"] = topicDictionary;

            return View();

        }
        public IActionResult Profile()
        {
            return View();
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
                        cmd.CommandText = $"Insert into contacts (UserId,id_newContact,username, username_newContact)" + $"values ( @UserId, @id_newContact,@username,@username_newContact) ";
                        cmd.Parameters.AddWithValue("@UserId", DBObject.m_id);
                        cmd.Parameters.AddWithValue("@id_newContact", id);
                        cmd.Parameters.AddWithValue("@username", DBObject.m_username);
                        cmd.Parameters.AddWithValue("@username_newContact", username);

                        cRead.Close();
                        cmd.Prepare();
                        cmd.ExecuteReader();
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
            getContacts.CommandText = "SELECT username_newContact FROM contacts where userID= @userID"; // the command
            getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);
            conn.Open();
            MySqlDataReader lRead = getContacts.ExecuteReader();
            List<string> ContactsList = new List<string>();

            while (lRead.Read())
            {
                ContactsList.Add(Convert.ToString(lRead[0]));
            }
            lRead.Close();

            homeMod.SetcontactsListAttr(ContactsList);
            conn.Close();
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

        public IActionResult PinnedMessages()
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            string txtcmd = $"Insert into united_messaging.pinnedMessages (userid, userName,topicgroupName, pinnedMessages)" + $"values ( @userID, @userName,@topicgroupName,@pinnedMessages)";
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@userID", DBObject.m_id);
            cmd.Parameters.AddWithValue("@userName", DBObject.m_username);
            //cmd.Parameters.AddWithValue("@topicgroupName", DBObject.m_TopicName);
            //cmd.Parameters.AddWithValue("@pinnedMessages", message);
            cmd.ExecuteNonQuery();
            conn.Close();

            return View();
        }
    }
}
