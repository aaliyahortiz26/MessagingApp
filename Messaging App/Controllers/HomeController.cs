using MessagingApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IActionResult PinnedMessages()
        {
            return View();
        }
    }
}
