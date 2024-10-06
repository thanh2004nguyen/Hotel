using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/room/amenities/theme/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminAmenitiesThemeController : MyBaseController
    {
        private readonly ILogger<AdminAmenitiesThemeController> _logger;

        public AdminAmenitiesThemeController(HotelDbContext context, ILogger<AdminAmenitiesThemeController> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var amenities = await _context.AmenitiesThemes.Include(i => i.IconClass).ToListAsync();
            return View(amenities);
        }
        public async Task<IActionResult> Create()
        {
            var icons = await _context.IconClasses.ToListAsync();
            ViewBag.IconList = icons;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AmenitiesTheme am)
        {
            // Clear validation for the IconClass property if necessary
            ModelState.Remove(nameof(AmenitiesTheme.IconClass));

            if (ModelState.IsValid)
            {
                _context.Add(am);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Log validation errors
            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                var errors = entry.Value.Errors;

                foreach (var error in errors)
                {
                    _logger.LogError($"Validation error for {key}: {error.ErrorMessage}");
                }
            }

            // Reload IconList if validation fails
            var icons = await _context.IconClasses.ToListAsync();
            ViewBag.IconList = icons;

            return View(am);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var property = await _context.RoomProperties.SingleOrDefaultAsync(a => a.Id == id);
            if (property == null)
            {
                return NotFound();
            }

            var icons = await _context.IconClasses.ToListAsync();
            ViewBag.IconList = icons;
            return View(property);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AmenitiesTheme am, int? selectedIconId)
        {
            if (ModelState.IsValid)
            {
                am.IconClassId = selectedIconId;
                _context.Entry(am).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var icons = await _context.IconClasses.ToListAsync();
            ViewBag.IconList = icons;
            return View(am);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var amenities = await _context.AmenitiesThemes.FindAsync(id);

            if (amenities == null)
            {
                return NotFound();
            }

            _context.Entry(amenities).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
