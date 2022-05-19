using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelSystem.DataAccessLayer.Controller;
using TravelSystem.DataAccessLayer.Database;
using TravelSystem.DataAccessLayer.Models;

namespace TravelSystem
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
            services.AddDbContextPool<AppDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLSERVER"));
            });
            services.AddIdentity<ApplicationUser, IdentityRole>(options=> 
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDBContext>();
            services.AddScoped<IDataControl, DataControl>();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/");
                options.LoginPath = new PathString("/");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
           

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            //for first time use
            CreateRoles(serviceProvider).Wait();
            CreateAdmin(serviceProvider).Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }

        public async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            ApplicationUser user = new ApplicationUser()
            {
                UserName = "Admin",
                Email = "admin@travel.com",
                EmailConfirmed = true,
                FirstName = "Amr",
                LastName = "Alaa"
            };

            var creationResult = await userManager.CreateAsync(user,"Admin1234");
            if (creationResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }           
        }
        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "Agency","Traveler" };
            foreach (var role in roles)
            {
                bool exist = await roleManager.RoleExistsAsync(role);
                if (!exist)
                {
                    await roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role
                    });
                }
            }
        }
    }
}
