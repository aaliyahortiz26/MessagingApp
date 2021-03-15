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
        public IActionResult Home(HomeModel homeMod)
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

            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }

        public IActionResult Preferences()
        {
            return View();
        }

        public IActionResult AccountSettings()
        {
            return View();
        }
        public IActionResult AccountSetting(AccountSettings account)
        {

                string Email = account.Email;
                String Username = account.userName;
                if (Email != null)
                {
                    const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
                    string txtcmd = "update users SET email ='" + account.Email + "' Where id ='" + DBObject.m_id + "'"; // the command

                    MySqlConnection conn = new MySqlConnection(connectionstring);
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    ViewBag.message = ("Email Changed");
                    DBObject.m_email = Email;
                    conn.Close();
                }
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
                return View("AccountSettings");
            }

        public IActionResult ChangePassword()
        {
            return View();
        }

    }
}