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

    public class LoginController : Controller
    {

        public IActionResult Index()
        {
            return View("Login");
        }

        [Route("Login")]
        public IActionResult Login()
        {
            if (ModelState.IsValid)
            {
                const string connectionstring = "server=localhost;user id=Guess; password = guess; persistsecurityinfo=True;database=message_app";
                MySqlConnection conn = new MySqlConnection(connectionstring);


                string Username = Request.Form["UserName"];
                string Password = Request.Form["Pass"];

                conn.Open();
                string txtcmd2 = $"SELECT* FROM userinfo where username = '" + Username + "' AND password = '" + Password + "'"; // the command
                MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
                MySqlDataReader dRead;

                using (dRead = cmd2.ExecuteReader()) // executes the search command
                {
                    if (dRead.Read())
                    {
                        conn.Dispose();
                        return View();

                    }
                }
                conn.Close();
            }

            return View();
        }

        [Route("CreateAccount")]
        public IActionResult CreateAccount()
        {

            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(CreateAccountModel CreateAccountModel)
        {

            if (ModelState.IsValid)
            {
                const string connectionstring = "server=localhost;user id=Guess; password = guess; persistsecurityinfo=True;database=message_app";
                MySqlConnection conn = new MySqlConnection(connectionstring);


                string Username = Request.Form["UserName"];
                string Password = Request.Form["Password"];
                string FirstName = Request.Form["FirstName"];
                string LastName = Request.Form["LastName"];
                string email = Request.Form["Email"];
                string PhoneNUm = Request.Form["Phone_Number"];
                string dob = Request.Form["DateOfBirth"];
                string ConfirmPassword = Request.Form["ConfirmPassword"];

                conn.Open();
                string txtcmd2 = $"SELECT id FROM users where username='" + Username + "'"; // the command
                MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
                MySqlDataReader dRead;

                using (dRead = cmd2.ExecuteReader()) // executes the search command
                {
                    if (dRead.Read())
                    {
                        conn.Close();
                        return View();
                    }
                }

                if (Username == "" || email == "" || FirstName == "" || LastName == "" || PhoneNUm == "" || dob == "")
                {


                    conn.Close();

                }

                if (Password != ConfirmPassword)
                {

                    conn.Close();
                }
                else if (Password == ConfirmPassword && !(Username == "" || email == "" || FirstName == "" || LastName == "" || PhoneNUm == "" || dob == ""))
                {
                    string txtcmd = $"Insert into users (firstName,lastName,username,password,email,phoneNUm, dob)" + $"values ( @firstName, @lastName,@username,@password,@email,@phoneNUm,@dob) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@firstName", FirstName);
                    cmd.Parameters.AddWithValue("@lastName", LastName);
                    cmd.Parameters.AddWithValue("@username", Username);
                    cmd.Parameters.AddWithValue("@password", Password);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phoneNUm", PhoneNUm);
                    cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(dob).ToString("yyyy-MM-dd"));
                    cmd.Prepare();
                    cmd.ExecuteReader();
                    conn.Close();
                }
            }
            return View();
        }

    }
}
