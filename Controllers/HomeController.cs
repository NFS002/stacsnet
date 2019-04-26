using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stacsnet.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace stacsnet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Subtitle = "An eLearning platform for St. Andrews Computer Science Students";
            ViewBag.Title = "Home";
            return View(new Account());
        }

        private string ErrorType() {
        string errorType = "";
        var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
        if (statusCodeReExecuteFeature != null)
        {
            errorType = statusCodeReExecuteFeature.OriginalPath;
            if ( errorType.StartsWith("/") ) {
                errorType = errorType.Substring(1);
            }
            int index = errorType.IndexOf("/");
            if (index != -1)
                errorType = errorType.Substring(0, index);
            
            }
            return errorType;
        }

        
        public IActionResult Error(int status = 500, string error_msg = "") {

            if (status == 401 && string.IsNullOrEmpty( error_msg ) ) 
                error_msg = "Please sign in or register";
            
            TempData[ "ErrMsg" ] = error_msg;
            TempData[ "Status" ] = status;
            
            return View();
        }

        private void Flash( string error_msg, string css ) {
            TempData[ "Flash" ] = error_msg;
            TempData[ "FlashCss" ] = css;
        }
    }
}