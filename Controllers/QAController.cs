using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using stacsnet.Models;
using stacsnet.Util;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 
using System.Diagnostics;


namespace stacsnet.Controllers
{
    [BasicAuthorize("stacsnet.com")]
    public class QAController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Comment(string return_url, QAPost post) {
            using (var context = new SnContext ()) {
                post.date = DateTime.Now;
                context.QAPosts.Add(post);
                context.SaveChanges();
                TempData["ErrMsg"] = "Comment posted";
                TempData["Css"] = "alert-success";
            }
            return Redirect(return_url); 
        }

        public IActionResult Index() {
            ViewBag.Title = "QA board";
            using (var context = new SnContext ()) {
                var posts = context.QAPosts.Where(p => p.pid == "0").ToList();
                QAThread thread = new QAThread();
                ViewBag.Subtitle = "QA board ";
                ViewBag.ButtonText = "Post a question or comment";
                ViewBag.CommentText = "posts";
                thread.posts = posts;

                thread.header = new QAPost {
                    pid = "0",
                    date = DateTime.MinValue,
                    text = "A threaded message board where you can anonymously ask questions about any of course content."
                    +
                    "Anyone may reply to your message. The posts below show the most recent threads.",
                    tags = "E.g,Java,OOP,Inheritance"
                };

                return View(thread);
            }
        }

        public IActionResult Thread(string pid) {
            ViewBag.Title = "QA board";
            using (var context = new SnContext ()) {
                var header= context.QAPosts.Where(p => p.id == pid).FirstOrDefault();
                if (header == null) {
                    var error_msg = "No matching QA posts were found";
                    TempData["ErrMsg"] = error_msg;
                    return RedirectToAction("Notfound","Home", new { status = 400 });
                }

                var posts = context.QAPosts.Where(p => p.pid == pid).ToList();
                QAThread thread = new QAThread();
                ViewBag.Subtitle = "QA board ";
                ViewBag.ButtonText = "Reply";
                ViewBag.CommentText = "replies";
                header.title = header.title;
                thread.header = header;
                thread.posts = posts;
                return View("Index", thread);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
