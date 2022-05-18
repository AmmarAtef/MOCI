using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MOCI.Core.Mapping;
using MOCI.DAL.DbContexts;
using MOCI.DAL.Interfaces;
using MOCI.DAL.Repositories;
using MOCI.Services;
using MOCI.Services.Interfaces;
using MOCI.Services.Services;
using MOCI.Web.Auth;
using MTRS.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MOCI.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MTRSDBContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MTRSDBContext).Assembly.FullName)));


          

            services.AddDbContext<LoggerDBContext>(options =>
           options.UseSqlServer(
               Configuration.GetConnectionString("MOCILogsConnection"),
               b => b.MigrationsAssembly(typeof(LoggerDBContext).Assembly.FullName)));

            services.AddAuthentication(
                   CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                   options =>
                   {
                       options.LoginPath = "/Account/Login";
                       options.LogoutPath = "/Account/Logout";
                   });

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
           
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //  services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
           services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));

            #region Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            
           
            services.AddScoped<IImportsRepository, ImportsRepository>();
            //services.AddScoped<ICustomerDataRepository, CustomerDataRepository>();


            services.AddScoped<ILogRepository, LogRepository>();

            services.AddScoped<IMappedColumnsRepository, MappedColumnsRepository>();
            
            #endregion

            #region Services
            services.AddScoped<IUserService, UserService>();
           
            services.AddScoped<IImportsService, ImportsService>();
            services.AddScoped<IFINHUB_REVENUE_HEADERService, FINHUB_REVENUE_HEADERDService>();
            services.AddScoped<IMappedColumnsService, MappedColumnsService>();
            services.AddScoped<ICustomerDataService, CustomerDataService>();


            #endregion

            services.AddScoped<MTRSDBContext, MTRSDBContext>();
            services.AddScoped<LoggerDBContext, LoggerDBContext>();
       
            services.AddScoped<UserManager, UserManager>();
            services.AddNotyf(config => { config.DurationInSeconds = 5; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

            services.AddControllersWithViews();

            // authentication 
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseNotyf();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
