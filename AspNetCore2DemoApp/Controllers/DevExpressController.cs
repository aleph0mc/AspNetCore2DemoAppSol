using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore2DemoApp.Controllers
{
    public class DevExpressController : Controller
    {
        public IActionResult DevExpressGrid()
        {
            return View("DevExpressGrid");
        }
    }
}