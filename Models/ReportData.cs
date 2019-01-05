using System;
using System.Linq;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel; 
using System.Collections.Generic;
using stacsnet.Util;

namespace stacsnet.Models
{
    public class ReportData {

        public List<ArrayList> reports { get; set; }

        public int count { get; set; }

        public int min { get; set; }

    }
}