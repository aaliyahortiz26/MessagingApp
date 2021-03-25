﻿using MessagingApp.Models;
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



            // set background color and textcolor that user selected
           
            const string connectionstring2 = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn2 = new MySqlConnection(connectionstring2);

            conn2.Open();

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
                pc.Baccolor = "White";

                string txtcmd = $"Insert into preferences (id,fontColor,backgroundColor)" + $"values ( @id, @TextColor,@BackgroundColor) ";
                MySqlCommand cmd = new MySqlCommand(txtcmd, conn2);
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
                     
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Contacts(HomeModel homeMod)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);

            conn.Open();

            MySqlCommand getContacts = conn.CreateCommand();
            getContacts.CommandText = "SELECT username FROM contacts where userID= @userID"; // the command
            getContacts.Parameters.AddWithValue("@userID", DBObject.m_id);
            MySqlDataReader reader = getContacts.ExecuteReader();

            List<string> ContactsList = new List<string>();

            while (reader.Read())
            {
                ContactsList.Add(Convert.ToString(reader[0]));
            }
            reader.Close();

            homeMod.SetcontactsListAttr(ContactsList);

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

        public IActionResult PinnedMessages()
        {
            return View();
        }
    }
}
