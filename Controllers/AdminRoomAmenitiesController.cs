using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/room/amenities/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminRoomAmenitiesController : MyBaseController
    {
        private readonly ILogger<AdminAmenitiesThemeController> _logger;

        public AdminRoomAmenitiesController(HotelDbContext context, ILogger<AdminAmenitiesThemeController> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var amenities = await _context.Amenities.Include(i => i.IconClass).Include(t=>t.AmenitiesTheme).ToListAsync();
            return View(amenities);
        }

        public async Task<IActionResult> Create()
        {
            var icons = await _context.IconClasses.ToListAsync();
            var themes = await _context.AmenitiesThemes.ToListAsync();

            ViewBag.IconList = icons;
            ViewBag.ThemeList = themes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Amenities ameniti)
        {
            if (ameniti.IconClassId.HasValue)
            {
                ameniti.IconClass = await _context.IconClasses.FindAsync(ameniti.IconClassId.Value);
            }

            if (ameniti.AmenitiesThemeId.HasValue)
            {
                ameniti.AmenitiesTheme = await _context.AmenitiesThemes.FindAsync(ameniti.AmenitiesThemeId.Value);
            }
            ModelState.Remove(nameof(Amenities.IconClass));
            ModelState.Remove(nameof(Amenities.AmenitiesTheme));
            if (ModelState.IsValid)
            {
                _context.Add(ameniti);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var icons = await _context.IconClasses.ToListAsync();
            var themes = await _context.AmenitiesThemes.ToListAsync();

            ViewBag.IconList = icons;
            ViewBag.ThemeList = themes;
            return View(ameniti);
        }




        public async Task<IActionResult> Delete(int id)
        {
            var amenity = await _context.Amenities.FindAsync(id);
            if (amenity == null)
            {
                return NotFound();
            }
            _context.Amenities.Remove(amenity);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {

            var uni = await _context.Amenities.SingleOrDefaultAsync(a => a.Id == id);
            return View(uni);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Amenities unity)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(unity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Redirect("Index");

            }
            else
            {
                var U = await _context.Amenities.AsNoTracking().SingleOrDefaultAsync(a => a.Id == unity.Id);
                if (unity.Name == U.Name)
                {
                    return Redirect("Index");

                }
                return View(unity);
            }



        }
    }
}
