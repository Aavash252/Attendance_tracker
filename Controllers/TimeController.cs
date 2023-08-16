using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FinalProject.Areas.Identity.Data;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace FinalProject.Controllers
{
    public class TimesController : Controller
    {
        private readonly FinalProjectDbContext _context;
        private readonly UserManager<FinalProjectUser> _userManager;

        public TimesController(FinalProjectDbContext context, UserManager<FinalProjectUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var finalprojectDbContext = _context.TimeModel.Include(t => t.FinalProjectUser);
            return View(await finalprojectDbContext.ToListAsync());
        }


        [HttpGet]

        // GET: Times/Create
        public IActionResult Create()
        {
            string userId = _userManager.GetUserId(User);
            ViewData["UserId"] = userId;

            // Create a new TimeTable object and set its UserId property
            TimeTable time = new TimeTable
            {
                UserId = userId,
                Date = DateTime.Now.Date
            };

            return View(time);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Date, Clock_In, Clock_Out")] TimeTable time)
        {
            string userid = _userManager.GetUserId(User);

            ViewData["UserId"] = userid;

            if (ModelState.IsValid)
            {
               time.UserId=userid;

                time.Date = DateTime.Now.Date;

                // Combine date and time components for Clock_In
                var combinedClockInDateTime = time.Date.Date + time.Clock_In.TimeOfDay;
                time.Clock_In = combinedClockInDateTime;

                // Combine date and time components for Clock_Out
                if (time.Clock_Out.HasValue)
                {
                    var combinedClockOutDateTime = time.Date.Date + time.Clock_Out.Value.TimeOfDay;
                    time.Clock_Out = combinedClockOutDateTime;
                }
                else
                {
                    time.Clock_Out = null;
                }

                _context.Add(time);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", time.Id);
            return View(time);
        }
        

    }
}
