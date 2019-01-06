using System;
using System.Net;
using System.Net.Mail;
using System.Linq;
using System.Diagnostics;
using stacsnet.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using stacsnet.Models;

namespace stacsnet.Controllers
{
    public class RegisterController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Submit(Account account)
        {
            string key = Guid.NewGuid().ToString();
            using (var acontext = new SnContext() ) {
                var model = new Account();
                var old_account = acontext.Accounts.FirstOrDefault( a => a.email == account.email);
                if (old_account != null) {
                    if (old_account.verified) {
                        TempData["RegisterMsg"] = "The email address " + account.email
                        + " has been registered and confirmed. The username and password you supplied when creating the account"
                        + " are valid to authenticate you on this site." ;
                        TempData["Css"] = "alert-info";
                    }
                    else {
                        TempData["RegisterMsg"] = "The email address " + account.email + "@st-andrews.ac.uk " + "has already submitted for registration. " 
                        + "Please check your emails from stacnet@gmail.com and open the confirmation link supplied.";
                        TempData["Css"] = "alert-warning";
                    }
                }
                else {
                    Send(account.email,account.uname, key);
                    var hasher = new PasswordHasher<Account>();
                    account.key = key;
                    account.pwhash = hasher.HashPassword(account, account.pwhash);
                    account.verified = false;
                    acontext.Accounts.Add(account);
                    acontext.SaveChanges();
                    TempData["RegisterMsg"] = "The email address " + account.email + "@st-andrews.ac.uk" + " was succesfully submitted for registration. "
                    + "Please check your emails from stacsnet@gmail.com and open the confirmation link supplied.";
                    TempData["Css"] = "alert-info";
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Confirm(string key)
        {
            ViewBag.Title = "confirm your account";
            
            using (var acontext = new SnContext()) {
                var account = acontext.Accounts.FirstOrDefault( a => a.key == key);
                if (account == null) {
                    TempData["RegisterMsg"] = "Your account was not registered or could not be found. Please submit "
                    + "for registration again.";
                    TempData["Css"] = "alert-danger";
                }
                else {
                    if (account.verified == false) {
                        account.verified = true;
                        acontext.Accounts.Update(account);
                        acontext.SaveChanges();
                    }
                    TempData["RegisterMsg"] = "The account " + account.email + "@st-andrews.ac.uk " + "(" 
                    + account.uname + ") " + "has been succesfuly registered and confirmed. Please save your password, "
                    + "you are authenticated to access all features of stacsnet";
                    TempData["Css"] = "alert-success";
                }
            }         
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Error()
        {
            ViewBag.Title = "Error";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [NonAction]
        private void Send(string email, string uname, string key) {

            var fromAddress = new MailAddress("stacsnet@gmail.com", "Stacsnet Admin");
            var toAddress = new MailAddress(email + "@st-andrews.ac.uk", uname);
            const string fromPassword = "staccy365";
            const string subject = "Verify your stacsnet account";
            string href = Static.url + "/Register/" + key;
            string body = System.IO.File.ReadAllText("Views/Shared/_Email.html")
                                    .Replace("{{uname}}",uname)
                                    .Replace("{{href}}", href);
                                    
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}
