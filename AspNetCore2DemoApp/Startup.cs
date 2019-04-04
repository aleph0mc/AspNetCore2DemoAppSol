using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore2DemoApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using AspNetCore2DemoApp.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.ResponseCompression;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using Microsoft.AspNetCore.DataProtection;
using System.IO;
using System.Security.Principal;

namespace AspNetCore2DemoApp
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Start ConfigureServices...");

            // Add the Response Compression
            //Brotli Compression Provider must be added when any compression providers are explicitly added
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { "image/svg+xml" });
            });

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Configuration.GetSection("ApiKeys").GetValue<string>("PersistKeysToFileSystem")))
                .ProtectKeysWithDpapi()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(50));

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>().AddDefaultUI(UIFramework.Bootstrap4).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.ConfigureClientsideValidation(enabled: false)) //Install-Package FluentValidation.AspNetCore
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Adds a default in-memory implementation of IDistributedCache
            //services.AddDistributedMemoryCache();
            services.AddMemoryCache();

            // Add Session management
            var sessionTimeout = Configuration.GetSection("Session").GetValue<int>("Timeout");
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(sessionTimeout);
            });

            services.AddTransient<IValidator<PersonVm>, PersonValidator>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _logger.LogInformation("End ConfigureServices...");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.LogInformation("Start Configure...");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseResponseCompression();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            // IMPORTANT: This session call MUST go before UseMvc()
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Important: this must be after app.UseMvc
            app.UseCookiePolicy();

            _logger.LogInformation("End Configure...");
        }
    }
}
