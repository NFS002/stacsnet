using System;
using System.IO;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel; 

namespace stacsnet.Models
{
    public class DataViewModel {

        public DirectoryInfo dir;

        public QAThread thread;
    }
}