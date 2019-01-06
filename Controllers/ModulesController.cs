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

namespace stacsnet.Controllers
{
    [BasicAuthorize("stacsnet.com")]
    public class ModulesController : Controller
    {

        //POST: /Upload
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files, string target_path, string return_url) {
            long size = files.Sum(f => f.Length);
            foreach (var file in files) {
                if (file.Length > 0) {
                    string file_name = file.FileName;
                    if (!String.IsNullOrEmpty(file_name)) {
                        string full_path = Path.Combine(target_path, file_name);
                        using (var stream = new FileStream(full_path, FileMode.Create)) {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else {
                        TempData["ErrMsg"] = "Some files could not be uploaded. "
                        + "Please check each file has a valid file name and extenion and try again.";
                        TempData["Css"] = "alert-danger";
                    }
                }
            }
            // process uploaded files
            // Don't rely on or trust the FgerleName property without validation.
            return Redirect(return_url);
        }

        // GET: /Modules
        public IActionResult All_years()
        {
            ViewBag.Title = "Years";
            ViewBag.Subtitle = "Available years";
            var dir = new DirectoryInfo(Static.MountPoint);

            string error_msg = "No years were found";

            if (!dir.Exists) // Error
                return RedirectToAction("Notfound", "Home", new { status = 404,
                                                                msg = error_msg } );

            return View(dir);
        }

        public IActionResult All_modules(string year)
        {
            ViewBag.Title = "Modules (" + year + ")";
            string path = Path.Combine(Static.MountPoint, year);
            DirectoryInfo dir = new DirectoryInfo(path);
            string error_msg = "No available modules were found ("
                                + year
                                + ")";

            if (!dir.Exists) {
                TempData["ErrMsg"] = error_msg;
                return RedirectToAction("Notfound", "Home", new { status = 404 } );
            }

            ViewBag.Subtitle = "Currenly available modules (" + year + ")";
            return View(dir);
        }


        public IActionResult All_resource_types(string module_code, string year)
        {
            ViewBag.Title = "Resources for " + module_code + " (" + year + ")";
            string path = Path.Combine(Static.MountPoint, year, module_code);
            DirectoryInfo dir = new DirectoryInfo(path);
            string error_msg = "No available entries were found for " 
                                + module_code 
                                + " (" 
                                + year
                                + ")";

            if (!dir.Exists) { // Error
                TempData["ErrMsg"] = error_msg;
                return RedirectToAction("Notfound", "Home", new { status = 404 } );
            }

            ViewBag.Subtitle = "Resources for " + module_code + " (" + year + ")";
            return View(dir);
        }


        public IActionResult All_files(string module_code, string year, string folder)
        {
            ViewBag.Title = folder + " for " + module_code + " (" + year + ")";
            string path = Path.Combine(Static.MountPoint, year, module_code, folder);
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists) {
                string error_msg = "The folder "
                                + folder
                                + " was not found in "
                                + module_code 
                                + " (" 
                                + year
                                + ")";
                
                TempData["ErrMsg"] = error_msg;                
                return RedirectToAction("Notfound", "Home", new { status = 404 } );
            }
            ViewBag.Subtitle = folder + " for " + module_code + " (" + year + ")";
            return View(dir);
        }

        public IActionResult File_content(string module_code, string year, string folder, string filename)
        {
            ViewBag.Title = filename;
            string full_path = Path.Combine(Static.MountPoint, year, module_code, folder, filename);

            if (!System.IO.File.Exists(full_path)) {
                string error_msg = filename
                                + " was not found in " 
                                + folder 
                                + " for "
                                + module_code 
                                + " (" 
                                + year
                                + ")";
                TempData["ErrMsg"] = error_msg;
                return RedirectToAction("Notfound", "Home", new { status = 404 } );
            }
            string ext = Path.GetExtension(full_path);
            string type = MimeTypeMap.GetMimeType(ext);

            var stream = new FileStream(@full_path, FileMode.Open);
            return new FileStreamResult(stream, type);  
        }
    }
}
