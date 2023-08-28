using FinalProject.Areas.Identity.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FinalProjectDbContext _context;
        private readonly UserManager<FinalProjectUser> _userManager;
        public HomeController(ILogger<HomeController> logger, FinalProjectDbContext context, UserManager<FinalProjectUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(User);
            

            ViewBag.CurrentUserId = userId;
            return View();
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
    }
}