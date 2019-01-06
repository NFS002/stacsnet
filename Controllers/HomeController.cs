using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stacsnet.Models;

namespace stacsnet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Subtitle = "An eLearning platform for St. Andrews Computer Science Students";
            ViewBag.Title = "Home";
            if (TempData["RegisterMsg"] == null) {
                ViewBag.RegisterMsg = "Your username and password do not have to be associated with your university account" +
                ", but you must provide a valid university email address to register. " +
                "Any posts or submissions you make on stacsnet can be anonymous or under a different username. Creating this account authenticates you "
                + " to access the site itself.";
                ViewBag.Css = "alert-info";
            }
            else {
		ViewBag.Subtitle = "TempData is Null";    
                ViewBag.RegisterMsg = TempData["RegisterMsg"].ToString();
                ViewBag.Css = TempData["Css"].ToString();
            }
            return View(new Account());
        }

        public IActionResult Notfound(string status)
        {
            ViewBag.Title = "Error";   
            ViewBag.Subtitle = status;

            if (TempData.ContainsKey("ErrMsg"))
                ViewBag.ErrMsg = TempData["ErrMsg"];
                
            else 
                ViewBag.ErrMsg = "Something happened...";

            return View();
        }

        public IActionResult Error()
        {
            ViewBag.Title = "Error"; 
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
