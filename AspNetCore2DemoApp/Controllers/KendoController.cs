using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore2DemoApp.ViewModels;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore2DemoApp.Controllers
{
    public class KendoController : Controller
    {
        public IActionResult KendoGridDemo()
        {
            return View("KendoGridDemo");
        }

        public async Task<IActionResult> LoadData([DataSourceRequest] DataSourceRequest request)
        {
            var gridInfo = new List<PersonVm>();
            for (int i = 0; i < 10; i++)
            {
                var p = new PersonVm
                {
                    Name = $"NTest_{i.ToString()}",
                    Lastname = $"LTest_{i.ToString()}",
                    DoB = new DateTime(1960, 1 + i, 10 + i)
                };

                gridInfo.Add(p);
            }


            var gridData = new DataSourceResult { Data = gridInfo, Total = gridInfo.Count };
            var jsonResult = Json(gridData);
            return jsonResult;
        }

    }
}
