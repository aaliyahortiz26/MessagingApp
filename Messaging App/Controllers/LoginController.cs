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
using System.Net.Mail;

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
            
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAccount(CreateAccountModel Ca)
        {

            if (ModelState.IsValid)
            {
                const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
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
                        ViewBag.message = "UserName Already Taken";
                        return View("CreateAccount");
                        
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



        public IActionResult ForgotPasswordScreen()
        {
            return View("ForgotPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ForgotPassword(ForgotPasswordModel fpm)
        {
            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            MySqlConnection conn = new MySqlConnection(connectionstring);


            string Username = fpm.Username;
            string Email = fpm.Email;
            string code = "";

            conn.Open();
            string txtcmd2 = $"SELECT* FROM users where username = '" + Username + "' AND email = '" + Email + "'"; // the command
            MySqlCommand cmd2 = new MySqlCommand(txtcmd2, conn);

            MySqlDataReader dRead;

            if (ModelState.IsValid)
            {
                using (dRead = cmd2.ExecuteReader()) 
                {
                    if (dRead.Read())
                    {
                      
                       int c = ' ';
                        Random rnd = new Random();
                        for (int i = 0; i < 6; i++)
                        {
                            c = rnd.Next(10);
                            code = code + c;
                        }
                       // fpm.RandomCode = code;
                        fpm.RandomCode = fpm.Username;

                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smpt.gmail.com");
                        mail.From = new MailAddress("unitedmessaging1@gmail.com");
                        mail.To.Add(fpm.Email);
                        mail.Subject = "Password Change Security Code";
                        mail.Body = $" The Security Code is : ${fpm.RandomCode}";
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("unitedmessaging1@gmail.com", "cs204SP21");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);

                        conn.Close();
                        dRead.Close();
                    }
                    else
                    {
                        conn.Close();
                        dRead.Close();
                        // Error message pops up on screen
                        ViewBag.message = "Username or Email not found!";
                    }

                }

            }
           return View("ForgotPassword");
        }

        public IActionResult Checker(ForgotPasswordModel ck)
        {
            if (ck.SecurityCode == ck.RandomCode)
            {
                return View("LoginChangePassword");
            }

            else
            {
                return View("LoginChangePassword");
            }
        }

        public IActionResult LoginChangePassword(LoginChangePasswordModel lcp)
        {
            string Password = lcp.NewPassword;
            string ConPassword = lcp.ConfirmPassword;
            string username = lcp.userName;

            const string connectionstring = "server=unitedmessaging.cylirx7dw3jb.us-east-1.rds.amazonaws.com;user id=Unitedmessaging; password = unitedmessaging21; persistsecurityinfo=True;database= united_messaging";
            string txtcmd = $"update userinfo SET password ='" + Password + "' Where username ='" + username + "'"; // the command
            MySqlConnection conn = new MySqlConnection(connectionstring);
            MySqlCommand cmd = new MySqlCommand(txtcmd, conn);
            conn.Open();
            cmd.ExecuteNonQuery();

            ViewBag.message = ("Password Changed");

            conn.Close();

            return View("Login");
        }

    }
}
