using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteHealthChecker.Models;
using SiteHealthChecker.Services;

namespace SiteHealthChecker.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IWebResourceService _webResourceService;

        public AdminController(IWebResourceService webResourceService)
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

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWebResource(WebResource newWebResource)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var successful = await _webResourceService.CreateWebResourceAsync(newWebResource);
            if (!successful)
            {
                return BadRequest(new { error = "Could not add web resource" });
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var webResource = await _webResourceService.GetWebResourceAsync(id);
            
            return View(webResource);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WebResource webResource)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            var successful = await _webResourceService.UpdateWebResourceAsync(webResource);
            if (!successful)
            {
                ModelState.AddModelError(string.Empty, "Could not update web resource.");
                return View(webResource);
            }
            return RedirectToAction("Index");   
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWebResource(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var successful = await _webResourceService.DeleteWebResourceAsync(id);
            if (!successful)
            {
                ModelState.AddModelError(string.Empty, "Could not delete web resource.");
                return View("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
