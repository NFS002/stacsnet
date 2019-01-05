using Microsoft.AspNetCore.Mvc;   
using System;
using System.Linq;
using System.Collections; 
using System.Collections.Generic;

namespace stacsnet.Models
{
    public class QAThread : IEnumerable<QAPost>
    {
        public IEnumerable<QAPost> posts { get; set; }

        public QAPost header { get; set; }

        public QAThread() => this.posts = Enumerable.Empty<QAPost>();
    
        public IEnumerator<QAPost> GetEnumerator() {
            return posts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}