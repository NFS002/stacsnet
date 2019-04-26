using System;
using System.Text.RegularExpressions;
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
                        if ( !System.IO.File.Exists( full_path) ) {
                            using (var stream = new FileStream(full_path, FileMode.Create)) {
                                await file.CopyToAsync(stream);
                            }
                        }
                    }
                    else {
                        Flash( "Some files could not be uploaded.  Please check each file has a valid file name and extenion and try again", "danger");
                    }
                }
            }
            return Redirect(return_url);
        }

        //POST: /ModuleEntry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModuleEntry( ModuleEntry moduleEntry ) {

            if (ModelState.IsValid ) {
                string year = moduleEntry.Year;
                string module = moduleEntry.Module;
                string folder = moduleEntry.Folder;
                string full_path = Path.Combine( Static.MOUNT, year, module, folder );
                DirectoryInfo dir = new DirectoryInfo( full_path );
                dir.Create();
                return All_files( module, year, folder );
            }

            else 
                return Error( 400, "Invalid module entry" );
            
        }

        // GET: /Modules
        public IActionResult All_years()
        {
            ViewBag.Title = ViewBag.Subtitle = "Modules";
            var dir = new DirectoryInfo( Static.MOUNT );
            if ( !dir.Exists ) 
                return RedirectToRoute( "Error" );
            return View( dir );
        }

        public IActionResult All_modules(string year)
        {
            string error_year = "'" + year + "'";
            
            if ( !Static.isYear( year ) ) {
                error_year = year.Substring(0,4) + "...";
                string error_msg = "No modules were found in " + error_year;
                Flash( error_msg, "danger" );
                return RedirectToAction( "All_years" );
            }
            else {
                string path = Path.Combine( Static.MOUNT, year);
                DirectoryInfo dir = new DirectoryInfo( path );
    
                if ( !dir.Exists ) {
                     string error_msg = "No modules were found in " + error_year;
                    Flash( error_msg, "danger" );
                    return RedirectToAction( "All_years" );
                }
                else {
                    ViewBag.Title = ViewBag.Subtitle = "Modules (" + year + ")";
                    return View( dir );
                }
            }
        }


        public IActionResult All_resource_types(string module_code, string year)
        {
            string error_year = year;
            string default_year = year;
            string error_module_code = module_code;
        
            
            if ( year.Length > 4 ) {
                error_year = year.Substring(0,4) + "...";
                default_year = Static.YEARS.First();
            }
            
            
            if ( module_code.Length > 6 )
                error_module_code = error_module_code.Substring(0, 6);
            
            

            if ( !Static.isYear( year ) ||
                 !Static.isModule( module_code) ) {
                string error_msg = "No resources were found for " + error_module_code + " (" + error_year + ")";
                Flash( error_msg, "danger" );
                return RedirectToAction( "All_modules", new { year = default_year } );
            }



            string path = Path.Combine(Static.MOUNT, year, module_code);
            DirectoryInfo dir = new DirectoryInfo( path );

            if ( !dir.Exists ) { /* Error */
                string error_msg = "No resources were found for " + error_module_code + " (" + error_year + ")";
                Flash( error_msg, "danger" );
                return RedirectToAction( "All_modules", new { year = default_year } );
            }
            else {
                ViewBag.Title = ViewBag.Subtitle = "Resources (" + module_code + ", " + year + ")";
                return View( dir ); 
            }  
        }


        public IActionResult All_files(string module_code, string year, string folder)
        {
            string error_year = year;
            string default_year = year;
        
            string error_module_code = module_code;
            string default_module_code = Static.MODULES.First();

            string error_folder = folder;
        
            if ( year.Length > 4 ) {
                error_year = year.Substring(0,4) + "...";
                default_year = Static.YEARS.First();
            }
            
            if ( module_code.Length > 6 )
                error_module_code = error_module_code.Substring(0, 6);

            if ( folder.Length > 10 )
                error_folder = error_folder.Substring(0, 10);
            
            if ( !Static.isYear( year ) ||
                 !Static.isModule( module_code) ||
                 !Static.isFolder( folder ) ) {
                    string error_msg = "No resources were found for " + error_module_code + " (" + error_year + ")";
                    Flash( error_msg, "danger" );
                    return RedirectToAction( "All_resource_types", new { year = default_year, module_code = default_module_code } );
            }

            string path = Path.Combine(Static.MOUNT, year, module_code, folder);
            DirectoryInfo dir = new DirectoryInfo( path );

            if ( !dir.Exists ) {
                string error_msg = "No resources were found in " + folder + " (" + module_code + ", " + year + ")";
                Flash( error_msg, "danger" );
                return RedirectToAction( "All_resource_types", new { year = default_year, module_code = default_module_code } );
            }
            else {
                ViewBag.Title = ViewBag.Subtitle = folder +  " (" + module_code + ", " + year + ")";
                return View( dir ); 
            } 
        }

        public IActionResult File_content(string module_code, string year, string folder, string filename)
        {
            ViewBag.Title = filename;
            string full_path = Path.Combine(Static.MOUNT, year, module_code, folder, filename);

            if ( !System.IO.File.Exists(full_path)) 
                return Error( 404, "File not found" );
            
            else {
                string ext = Path.GetExtension( full_path );
                string type = MimeTypeMap.GetMimeType( ext );
                FileStream stream = new FileStream( @full_path, FileMode.Open );
                return new FileStreamResult( stream, type );
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
