﻿using System;
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
        [Authorize]

        // GET: Times/Create
        public IActionResult Create()
        {
            string userId = _userManager.GetUserId(User);
            ViewData["UserId"] = userId;





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


#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            TimeTable clockInRecord = await _context.TimeModel
                .Where(t => t.UserId == userId && t.Clock_Out == null)
                 .OrderByDescending(t => t.Clock_In)
                     .FirstOrDefaultAsync();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

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



        [HttpGet]
        public async Task<IActionResult> GetRecentClockIn()
        {
            string userId = _userManager.GetUserId(User);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            TimeTable recentClockIn = await _context.TimeModel
                .Where(t => t.UserId == userId && t.Clock_Out == null)
                .OrderByDescending(t => t.Clock_In)
                .FirstOrDefaultAsync();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            if (recentClockIn != null)
            {
                DateTime clockInTime = recentClockIn.Clock_In;

                return Json(new { success = true, clockInTime = clockInTime.ToString("yyyy-MM-dd HH:mm:ss") });
            }
            else
            {
                return Json(new { success = false, message = "No recent login found." });
            }
        }

    }
}
