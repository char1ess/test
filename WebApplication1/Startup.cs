using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Middlewares;
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
            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;//是否包括非字母的数字字符（默认为true）
                option.Password.RequireUppercase = false;//是否包括大写字母
            });//重新设置验证密码的配置
            services.ConfigureApplicationCookie(option=>{ option.AccessDeniedPath = "/Account/fangyu"; });//授权认证不正确跳转防御页面
            services.AddIdentity<ApplicationUser, IdentityRole>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>();//添加实体框架存储
            services.AddMvc(config =>
            {//新增全局授权身份认证
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
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
                app.UseExceptionHandler("/Error");//拦截内部异常
                //拦截404未找到异常
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");//将地址修改为/Error/404，错误码改为302
                app.UseStatusCodePagesWithReExecute("/Error/{0}");//保留源地址，错误码一直为404
            }
            app.UseStaticFiles();
            app.UseAuthentication();//在使用MVC之前进行身份验证
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
