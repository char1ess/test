using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection")));
            services.AddMvc();
            services.AddScoped<IStudentRepository, SQLStudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else//非开发环境异常界面
            {
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");//将地址修改为/Error/404，错误码改为302
                app.UseStatusCodePagesWithReExecute("/Error/{0}");//保留源地址，错误码一直为404
            }
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            //app.UseMvc(config=>config.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"));
            //var processname = Process.GetCurrentProcess().ProcessName;
            //app.Use(async (context, next) => {context.Response.ContentType = "text/plain;charset=utf-8"; await context.Response.WriteAsync(processname); next(); });
            //app.Run(async (context) =>
            //{

            //    await context.Response.WriteAsync("Hello World!第二个中间件");
            //});
        }
    }
}
