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
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginModel lm)
        {
            
            const string connectionstring = "server=localhost;user id=root; password = MessagingApp; persistsecurityinfo=True;database=message_app";
            MySqlConnection conn = new MySqlConnection(connectionstring);


            string Username = lm.Name;
            string Password = lm.Pass;

            conn.Open();
            string txtcmd2 = $"SELECT* FROM users where username = '" + Username + "' AND password = '" + Password + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
          
            MySqlDataReader dRead;

            if (ModelState.IsValid)
            {
                using (dRead = cmd2.ExecuteReader()) // executes the search command
                {
                    if (dRead.Read()) // Checks if username and password is in it
                    {

                        string txtcmd1 = "SELECT id FROM users where username='" + Username + "'"; // the command
                        MySqlCommand cmd1 = new MySqlCommand(txtcmd1, conn);
                        dRead.Close();
                        using (dRead = cmd1.ExecuteReader()) // executes the search command
                        {
                            if (dRead.Read())
                            {
                                lm.id = Convert.ToInt32(dRead.GetValue(0).ToString());
                                return View("~/Views/Home/Home.cshtml");
                            }
                        }
                        conn.Close();
                        dRead.Close();
                    }
                    else
                    {
                        conn.Close();
                        dRead.Close();
                        // Error message pops up on screen
                        ViewBag.message = "Username not found or password incorrect!";
                        return View("Login");
                    }

                }

            }
                return View("Login");
                
        }  

        public IActionResult CreateAccountScreen()
        {

            return View("CreateAccount");
        }

//        [Route("CreateAccount")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount(CreateAccountModel Ca)
        {

            if (ModelState.IsValid)
            {
                const string connectionstring = "server=localhost;user id=root; password = MessagingApp; persistsecurityinfo=True;database=message_app";
                MySqlConnection conn = new MySqlConnection(connectionstring);

                conn.Open();
                
                string txtcmd2 = $"SELECT id FROM users where username='" + Ca.UserName + "'"; // the command
                MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);
                MySqlDataReader dRead;
                using (dRead = cmd2.ExecuteReader()) // executes the search command
                {
                    if (dRead.Read())
                    {
                        conn.Close();
                        return View("CreateAccount.cshtml");
                    }
                }
                dRead.Close();

                if (Ca.Password != Ca.ConfirmPassword)
                {

                    conn.Close();
                    return View("CreateAccount.cshtml");
                } 
                
                else
                {
                    string txtcmd = $"Insert into users (firstName,lastName,username,password,email,phoneNUm, dob)" + $"values ( @firstName, @lastName,@username,@password,@email,@phoneNUm,@dob) ";
                    MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@firstName", Ca.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", Ca.LastName);
                    cmd.Parameters.AddWithValue("@username", Ca.UserName);
                    cmd.Parameters.AddWithValue("@password", Ca.Password);
                    cmd.Parameters.AddWithValue("@email", Ca.Email);
                    cmd.Parameters.AddWithValue("@phoneNUm", Ca.Phone_Number);
                    cmd.Parameters.AddWithValue("@dob", Convert.ToDateTime(Ca.DateOfBirth).ToString("yyyy-MM-dd"));
                    cmd.Prepare();
                    cmd.ExecuteReader();
                    conn.Close();
                    return View("Login");
                }
            }
            return View("CreateAccount");
        }

    }
}
