using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Diagnostics;
using stacsnet.Models;

namespace stacsnet.Util {
    public static class Static {
        public static IHostingEnvironment ENV { get; set; }
        public static string URL { get; set; }
        public static string MOUNT { get; set; }
        public static QAPost FIRST_POST { 
            get {
                return new QAPost {
                    pid = "0",
                    date = DateTime.MinValue,
                    text = "A threaded message board where you can anonymously ask questions about any of course content."
                    +
                    "Anyone may reply to your message. The posts below show the most recent threads.",
                    tags = "E.g,Java,OOP,Inheritance"
                };
            }        
        }
        public static List<string> TYPES { get; set; }
        private static List<string> _YEARS;
        public static List<string> YEARS { 

            get {
                loadYears();
                return _YEARS;
            }

            set {
                _YEARS = value;
            }

        }

        private static List<string> _MODULES;
        public static List<string> MODULES { 

            get {
                loadModules();
                return _MODULES;
            }
            set {
                _MODULES = value;
            }
            
        }

        public static void init( IConfiguration configuration, IHostingEnvironment env ) {

            ENV = env;

            URL = configuration.GetSection("URL").Value;

            MOUNT = configuration.GetSection( "MOUNT" ).Value;

            TYPES = configuration.GetSection( "TYPES" ).Get<string[]>().ToList();

            _YEARS = YEARS = configuration.GetSection( "YEARS" ).Get<string[]>().ToList();

            _MODULES = configuration.GetSection( "MODULES" ).Get<string[]>().ToList();

            mkDirs();

            loadDb( );
            

        }
        private static void loadDb( ) {
            var context = new SnContext ();
            context.Database.EnsureCreated();
        }
        

        public static void loadModules() {

            using(var context = new SnContext()) {
                var _givenmodules = context.GradeReports.Select( g => g.code )
                                                .Distinct()
                                                .ToList();

                 foreach (var m in _givenmodules)
                        _MODULES.Add(m);
            }

            DirectoryInfo parentDir = new DirectoryInfo( MOUNT );
            foreach(var dir in parentDir.GetDirectories()) {
                List<DirectoryInfo> modules_in_year = dir.GetDirectories().ToList();
                foreach (var module in modules_in_year)
                    _MODULES.Add(module.Name);
            }
            _MODULES = _MODULES.Distinct().ToList();
        }

        public static void loadYears( ) {

            using( var context = new SnContext() ) {
                var _givenYears = context.GradeReports.Select( g => g.Year )
                                        .Distinct()
                                        .ToList();

                foreach ( var y in _givenYears ) 
                    _YEARS.Add( y );
            }
            
            DirectoryInfo parentDir = new DirectoryInfo( MOUNT );

            foreach(var dir in parentDir.GetDirectories())
                _YEARS.Add( dir.Name );

            _YEARS = _YEARS.Distinct().ToList();

        }

        private static void mkDirs() {

        
            DirectoryInfo dir = new DirectoryInfo( MOUNT );
            dir.Create();

            foreach( string year in YEARS) {
                DirectoryInfo subdir =  dir.CreateSubdirectory( year );
                foreach( string module in MODULES) {
                    DirectoryInfo subsubdir = subdir.CreateSubdirectory( module );
                    foreach( string type in TYPES ) {
                        DirectoryInfo subsubsubdir = subsubdir.CreateSubdirectory( type );
                    }
                }
            }



            DirectoryInfo dbDir = new DirectoryInfo( "Db" );
            dbDir.Create();
            string dbPath = "Db/db.sqlite";

            if ( !File.Exists( dbPath ) ) 
                File.Create( dbPath );
        }

        public static bool isYear( int year ) {
            return Regex.IsMatch( year.ToString(),"^(19|20)[0-9]{2}$");

        }

        public static bool isYear( string year ) {
            return Regex.IsMatch( year,"^(19|20)[0-9]{2}$");
        }

        public static bool isModule( string module ) {
            return Regex.IsMatch( module,"^[A-Z]{2}[1-9][0-9]{3}$");
        }

        public static bool isFolder( string folder ) {
            int length = folder.Length;
            return length >= 2 && length <= 10;
        }

    }
}