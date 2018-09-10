using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteHealthChecker.Models;
using SiteHealthChecker.Services;

namespace SiteHealthChecker.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebResourceService _webResourceService;

        public HomeController(IWebResourceService webResourceService)
        {
            _webResourceService = webResourceService;
        }

        public async Task<IActionResult> Index()
        {
            var webResources = await _webResourceService.GetWebResourcesAsync();

            var model = new WebResourceViewModel
            {
                Items = webResources
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
