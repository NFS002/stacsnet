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

            ViewBag.Modules = Static.MODULES;
            ViewBag.Years = Static.YEARS;
        

            if ( !Static.isYear( year ) || !Static.isModule( module_code ) ) {
                string error_msg =  "Invalid selection " + "(" + module_code + ", " + year + ").";
                string default_module = Static.MODULES.First();
                string default_year = Static.YEARS.First();
                Flash(error_msg, "danger");
                return RedirectToAction( "Index", new { module_code = default_module, year = default_year} );

            }
            else {
                ViewBag.Module = module_code;
                ViewBag.Year = year;
                ViewBag.Title = "Grade reports";
                ViewBag.Subtitle = "Grade reports for " + module_code + " (" + year + ")";
                return View( loadReports( module_code, year ));
            }
        }


        [HttpPost]
        public IActionResult Submit(string return_url, GradeReport report) {
            if (ModelState.IsValid) {
                using(var context = new SnContext ()) {
                    context.GradeReports.Add(report);
                    context.SaveChanges();
                }
                Flash( "submission received", "success" );
            }
            else 
                Flash( "submission not received", "danger");
            
            return Redirect(return_url);
        }

        private Dictionary<string, ReportData> loadReports( string module_code, string year ) {
                using (var context = new SnContext()) {
                    var allreports = context.GradeReports.Where(g => g.code == module_code && g.Year == year).ToList();
                    var reports = new Dictionary<string, ReportData>();
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
                return reports;
            }
        }

        private IActionResult Error(int status, string error_msg ) =>
            RedirectToRoute( "Error", new { status = status, error_msg = error_msg } );

        private void Flash( string error_msg, string css ) {
            TempData[ "Flash" ] = error_msg;
            TempData[ "FlashCss" ] = css;
        }

    }
}
