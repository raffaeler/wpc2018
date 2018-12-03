using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthzDemoWeb.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using AuthorizationLibrary;

namespace AuthzDemoWeb
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Sqlite"))
                //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );

            //services.AddDefaultIdentity<IdentityUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddAuthentication()    // "DefaultScheme"
                .AddCookie()
                .AddFacebook(options =>
                {
                    options.ClientId = Configuration["FacebookLogin:Appid"];
                    options.ClientSecret = Configuration["FacebookLogin:AppSecret"];
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = Configuration["MSLogin:Appid"];
                    options.ClientSecret = Configuration["MSLogin:AppSecret"];
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration["GoogleLogin:AppId"];
                    options.ClientSecret = Configuration["GoogleLogin:AppSecret"];
                });


            services.RegisterHandlers();
            services.AddAuthorization(options => options.RegisterPolicies());

            services.AddSingleton<IClaimsTransformation, ClaimTransformer>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
