using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly FinalProjectDbContext _context;
        private readonly UserManager<FinalProjectUser> _userManager;
        public UserController(FinalProjectDbContext context, UserManager<FinalProjectUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [Authorize(Roles = "Admin")]

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult List()
        {
          

            var users = _userManager.Users.ToList();
            return View(users);


          
        }
        public IActionResult Graph(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            DateTime thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var timeRecords = _context.TimeModel
                .Where(t => t.UserId == userId && t.Date >= thirtyDaysAgo)
                .ToList();

            var groupedRecords = timeRecords
                .GroupBy(t => (t.Date - thirtyDaysAgo).Days + 1) 
                .Select(group => new
                {
                    Day = group.Key,
                    Date = thirtyDaysAgo.AddDays(group.Key - 1)
                        .ToString("yyyy-MM-dd"), 
                    TotalHours = group.Sum(record => (record.Clock_Out - record.Clock_In)?.TotalHours ?? 0)
                })
                .ToList();

           
            List<string> dates = groupedRecords.Select(record => record.Date).ToList();
            List<double> totalHours = groupedRecords.Select(record => record.TotalHours).ToList();

           
            ViewBag.Dates = dates;
            ViewBag.TotalHours = totalHours;

           
            return View("Graph");
        }








        public IActionResult Info(string userId)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Id == userId);




            if (user == null)
            {
                return NotFound();
            }

            var timeRecords = _context.TimeModel
                .Where(t => t.UserId == userId)
                .ToList();

            ViewBag.TimeRecords = timeRecords;

            return View(user);
        }





    }


}


