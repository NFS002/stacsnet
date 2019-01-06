using System;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

namespace stacsnet.Util {
    public static class Static {

        private static bool loaded = false;
        public static string url { get; set; }

        public static string MountPoint { get; set; }
        public static List<string> allYears { get; set; }

        public static List<string> givenYears { get; set; }

        public static List<string> allModules { get; set; }

        public static List<string> givenModules { get; set; }

        public static void init(IConfiguration configuration) {
            if (loaded)
                return;
            
            mkDirs();
            url = configuration["Url"];
            MountPoint = configuration["Mount"];

            string dbargs = configuration["Db"];
            if ( !string.IsNullOrEmpty(dbargs) && !dbargs.Contains("n"))
                refreshdb(dbargs);

            loadModules();
            loadYears();
            loaded = true;
        }
        private static void refreshdb(string args) {
            using (var context = new SnContext ()) {
                context.Database.EnsureCreated();
                foreach(char c in args) {
                    switch (c) {
                        case 't':
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        break;
                        case 'a':
                        context.Accounts.RemoveRange(context.Accounts);
                        break;
                        case 'q':
                        context.QAPosts.RemoveRange(context.QAPosts);
                        break;
                        case 'g':
                        context.GradeReports.RemoveRange(context.GradeReports);
                        break;
                    }
                }
            }

        }
        

        public static void loadModules() {
            allModules = givenModules = new List<string>();
            using(var context = new SnContext()) {
                var _givenmodules = context.GradeReports.Select( g => g.code ).ToList();
                foreach (var m in _givenmodules) {
                    if (!givenModules.Contains(m))
                        givenModules.Add(m);
                }
                allModules.AddRange(givenModules);
            }
            DirectoryInfo parentDir = new DirectoryInfo(Static.MountPoint);
            foreach(var dir in parentDir.GetDirectories()) {
                List<DirectoryInfo> modules_in_year = dir.GetDirectories().ToList();
                foreach (var module in modules_in_year) {
                    if (!allModules.Contains(module.Name))
                        allModules.Add(module.Name);
                }
            }
            if (!givenModules.Any())
                givenModules.AddRange(allModules);
        }

        public static void loadYears() {
            allYears = givenYears = new List<string>();
            using(var grcontext = new SnContext()) {
                var _givenyears = grcontext.GradeReports.Select(g => g.Year).ToList();
                foreach( var year in _givenyears) {    
                    if (!givenYears.Contains(year))
                        givenYears.Add(year);
                }
                allYears.AddRange(givenYears);
            }
            DirectoryInfo parentDir = new DirectoryInfo(Static.MountPoint);
            foreach(var dir in parentDir.GetDirectories()) {
                string year = dir.Name;
                if (!allYears.Contains(year))
                    allYears.Add(year);
            }
            if (!givenYears.Any())
                givenYears.AddRange(allYears);
        }

        private static void mkDirs() {
            DirectoryInfo dir = new DirectoryInfo(Static.MountPoint);

            if (!dir.Exists)
                dir.Create();

            dir = new DirectoryInfo("Db");

            if (!dir.Exists)
                dir.Create();
            
            if (File.Exists("Db/db.sqlite"))
                File.Create("Db/db.sqlite");

        }
    }
}