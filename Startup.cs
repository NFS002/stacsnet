using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using stacsnet.Util;
using Microsoft.AspNetCore.HttpOverrides;

namespace stacsnet
{
    public class Startup
    {

        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env )
        {
            Static.init( Configuration, env );

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStatusCodePagesWithReExecute("/Error","?status={0}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

        

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "File_upload",
                    "Upload",
                    new {
                        controller = "Modules",
                        action = "Upload" });
                    
                /* routes.MapRoute(
                    "Module_entry",
                    "ModuleEntry",
                    new {
                        controller = "Modules",
                        action = "ModuleEntry" }); */

                routes.MapRoute(
                    "RegisterEmail",
                    "Register",
                    new {
                        controller = "Register",
                        action = "Submit" });

                routes.MapRoute(
                    "ConfirmEmail",
                    "Register/{key}",
                    new {
                        controller = "Register",
                        action = "Confirm" });

                routes.MapRoute(
                    "QAComment",
                    "QA/Comment",
                    new {
                        controller = "QA",
                        action = "Comment" });
                
                routes.MapRoute(
                    "All_years",
                    "Modules",
                    new { controller = "Modules",
                          action = "All_years" });

                routes.MapRoute(
                    "All_modules",
                    "Modules/{year}",
                    new { controller = "Modules",
                          action = "All_modules" });

                routes.MapRoute(
                    "All_resource_types",
                    "Modules/{year}/{module_code}",
                    new { controller = "Modules",
                          action = "All_resource_types" });

                routes.MapRoute(
                    "All_files",
                    "Modules/{year}/{module_code}/{folder}",
                    new { controller = "Modules",
                          action = "All_files" });

                routes.MapRoute(
                    "File_content",
                    "Modules/{year}/{module_code}/{folder}/{filename}",
                    new { controller = "Modules",
                          action = "File_content" });

                routes.MapRoute(
                    "QAindex",
                    "QA",
                    new { controller = "QA",
                          action = "Index" });

                routes.MapRoute(
                    "QAthread",
                    "QA/{pid}",
                    new { controller = "QA",
                          action = "Thread" });


                routes.MapRoute(
                    "Report_Module_Choice",
                    "Report",
                    new { controller = "Modules",
                          action = "Index" });


                routes.MapRoute(
                    "Report_Submit",
                    "Report/Submit",
                    new { controller = "Report",
                          action = "Submit" });
                
                routes.MapRoute(
                    "Report",
                    "Report/{module_code}/{year}",
                    new { controller = "Report",
                          action = "Index" });

                routes.MapRoute(
                    "default",
                    "",
                    new { controller = "Home",
                          action = "Index" });

                routes.MapRoute(
                    "Error",
                    "Error",
                    new { controller = "Home",
                          action = "Error" });


            });
	    app.UseCookiePolicy();
        }
    }
}
