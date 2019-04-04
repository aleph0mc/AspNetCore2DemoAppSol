using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore2DemoApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimpleImpersonation;

namespace AspNetCore2DemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _domain;
        private string _username;

        private IConfiguration _configuration;


        private async Task<int> SaveToDB(PersonVm model)
        {
            return 0;
        }

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            var domainUser = _httpContextAccessor.HttpContext.User.Identity.Name;

            if (!string.IsNullOrEmpty(domainUser))
            {
                _domain = Regex.Replace(domainUser, "(.*)\\\\.*", "$1", RegexOptions.None);
                _username = Regex.Replace(domainUser, ".*\\\\(.*)", "$1", RegexOptions.None);
            }

            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInformation($"Username: {_username}");

            _logger.LogInformation("==================================================================");


            var credentials = new UserCredentials("domain", "user", "pwd");
            Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
            {
                string connectionString = _configuration.GetConnectionString("NwinDBConn");
                _logger.LogInformation($"Connecting to DB via trusted authentication (impersonation): {connectionString}");

                using (var dbConn = new SqlConnection(connectionString))
                {
                    _logger.LogInformation($"OPENING CONNECTION TO DB Northwind");

                    try
                    {
                        dbConn.Open();
                        _logger.LogInformation($"CONNECTION OPENED SUCCESSFULLY");
                    }
                    catch (Exception ex)
                    {

                        _logger.LogInformation($"CONNECTION NOT OPENED SUCCESSFULLY . . . ERROR: {ex.Message}");
                    }
                }
            });

            _logger.LogInformation("==================================================================");


            ViewBag.Username = _username;

            if (!string.IsNullOrEmpty(TempData["Success"]?.ToString()))
                ViewBag.Success = "1";

            return View();
        }

        // POST: Home/SavePersonAsync
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePersonAsync(/* [Bind("Name, Lastname, DoB, ")] */ PersonVm model)
        {
            if (ModelState.IsValid)
            {
                var ret = await SaveToDB(model);
                if (0 == ret)
                {
                    TempData["Success"] = "1";
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }

            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult PersonInfo()
        {
            return View("_personInfo");
        }
    }
}
