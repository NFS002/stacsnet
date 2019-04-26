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
                Flash( "Comment posted", "success");
            }
            return Redirect(return_url); 
        }

        public IActionResult Index() {
            var thread = findThread( "0" );
            ViewBag.Title = ViewBag.Subtitle = "QA board ";
            ViewBag.ButtonText = "Post a question or comment";
            ViewBag.CommentText = "posts";
            return View( thread );
        }

        public IActionResult Thread( string pid ) {
            if ( pid.Equals("0") ) 
                return RedirectToAction( "Index" );
            ViewBag.Title = ViewBag.Subtitle = "QA board";
            ViewBag.ButtonText = "Reply";
            ViewBag.CommentText = "replies";
            QAThread thread = findThread( pid );
            if ( thread == null ) {
                Flash( "No matching QA posts were found", "danger" );
                return RedirectToAction( "Index" );
            }
            else  
                return View( "Index", thread );

        }

        private IActionResult Error(int status, string error_msg ) =>
            RedirectToRoute( "Error", new { status = status, error_msg = error_msg } );

        private void Flash( string error_msg, string css ) {
            TempData[ "Flash" ] = error_msg;
            TempData[ "FlashCss" ] = css;
        }

        private QAThread findThread( string pid ) {
            QAPost header = null;
            List<QAPost> posts = new List<QAPost>();
            var context = new SnContext ();

            if ( pid.Equals("0")) 
                header = Static.FIRST_POST;
            else
                header = context.QAPosts.Where(p => p.id == pid).FirstOrDefault();
            
            if (header == null )
                return null;
            else {
                posts.AddRange( context.QAPosts.Where(p => p.pid == pid) );

                return new QAThread() {
                    header = header,
                    posts = posts
                };
            }
        }
    }
}
