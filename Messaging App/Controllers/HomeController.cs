using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagingApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Home");
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult TopicSearch()
        {
            return View();
        }
        public IActionResult CreateGroup()
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
