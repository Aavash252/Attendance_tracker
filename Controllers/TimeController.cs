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
using Microsoft.AspNetCore.Authorization;
using System.Data;

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


            bool hasClockInData = _context.TimeModel.Any(t => t.UserId == userId && t.Clock_In != DateTime.MinValue);

            ViewData["HasClockInData"] = hasClockInData;


            
            TimeTable time = new TimeTable
            {
                UserId = userId,
               
            };

            


            return View(time);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Date, Clock_In, Clock_Out")] TimeTable time)
        {
           

            


            time.Date= DateTime.Now;

            _context.Add(time);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var finalprojectDbContext = _context.TimeModel.Include(t => t.FinalProjectUser);
            return View(await finalprojectDbContext.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> ClockIn()
        {
            string userId = _userManager.GetUserId(User);

            TimeTable time = new TimeTable
            {
                UserId = userId,
                Date = DateTime.Now,
                Clock_In = DateTime.Now,
                Clock_Out = null
            };

            _context.Add(time);
            await _context.SaveChangesAsync();

            StatusMessage = "Clock In successful.";

            return Json(StatusMessage);
        }


        [HttpGet]
        public async Task<IActionResult> ClockOut()
        {
            string userId = _userManager.GetUserId(User);


            TimeTable clockInRecord = await _context.TimeModel
                .Where(t => t.UserId == userId && t.Clock_Out == DateTime.MinValue)
                 .OrderByDescending(t => t.Clock_In)
                     .FirstOrDefaultAsync();

            if (clockInRecord != null)
            {
                clockInRecord.Clock_Out = DateTime.Now;
                await _context.SaveChangesAsync();

                StatusMessage = "Clock Out successful.";
            }
            else
            {
                StatusMessage = "No clock-in record found for the user.";
            }

            return Json(StatusMessage);
        }



    }
}
