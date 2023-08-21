using FinalProject.Areas.Identity.Data;
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
        public IActionResult List()
        {
          

            var users = _userManager.Users.ToList();
            return View(users);


          
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


