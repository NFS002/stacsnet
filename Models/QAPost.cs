using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel; 

namespace stacsnet.Models
{
    [Bind("module_code","year","title","uname","text","tags","pid","replyname")] 
    public class QAPost
    {
        [DisplayName("Username ")]
        public string uname { get; set; }

        [Required]
        public DateTime date { get; set; }

        [DisplayName("Tags (comma seperated list) ")]
        public string tags { get; set; }

        [Required]
        [DisplayName("Content* ")]
        public string text { get; set; }

        [Required]
        [DisplayName("Title* ")]
        public string title { get; set; }

        [Key]
        [ScaffoldColumn(false)]
        public string id { get; set; }

        [Required]
        public string pid { get; set; }


        public QAThread Children() {
            QAThread thread = new QAThread();
            using (var context = new SnContext ()) {
                thread.posts = context.QAPosts.Where(p => p.pid == id).ToList();
                thread.header = this;
                context.SaveChanges();
            }
            return thread;
        }   
    }
}