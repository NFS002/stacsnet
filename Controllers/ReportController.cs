using System;
using System.Collections;
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

namespace stacsnet.Controllers
{
    [BasicAuthorize("stacsnet.com")]
    public class ReportController : Controller
    {
        [HttpGet]
        public IActionResult Index(string module_code, string year) {
            ViewBag.Title = "class averages for " + module_code + "(" + year + ")";
            using (var grcontext = new SnContext()) {
                var currentYear = DateTime.Now.Year;
                var allreports = grcontext.GradeReports.Where(g => g.code == module_code && g.Year == year).ToList();
                ViewBag.Module = module_code;
                ViewBag.Modules = Static.givenModules;
                ViewBag.Years = Static.givenYears;
                ViewBag.Year = year;        
                ViewBag.Subtitle = "Grade Reports for " + module_code + " (" + year + ")";
                var reports = new Dictionary<string,ReportData>();
                var mincount = 3;
                foreach (var type in Enum.GetNames(typeof(GradeType))) {
                    var typedreports = allreports.Where(g => g.Type.ToString() == type).ToList();
                    var count = typedreports.Count();
                    if (count < mincount) {
                       reports.Add(type, new ReportData {
                            count = count,
                            min = mincount,
                            reports = new List<ArrayList>()
                        }); 
                    }
                    else {
                        var list = new List<ArrayList>();

                        foreach (var row in typedreports) {
                            var innertype = type;
                            if (type == "Practical")
                                innertype = "W" + row.Week;
                            list.Add( new ArrayList{ innertype, row.Grade } );
                        }

                        reports.Add(type, new ReportData {
                            count = count,
                            min = mincount,
                            reports = list
                        });
                    }
                }
                return View(reports);
            }
        }


        [HttpPost]
        public IActionResult Submit(string return_url, GradeReport report) {
            if (ModelState.IsValid) {
                using(var context = new SnContext ()) {
                    context.GradeReports.Add(report);
                    context.SaveChanges();
                }
                TempData["Msg"] = "Submission received";
                TempData["Css"] = "alert alert-success";
            }   
            else {
                TempData["Msg"] = "Submission not received, please try again";
                TempData["Code"] = "alert alert-danger";
            }
            return Redirect(return_url);
        }
    }
}
