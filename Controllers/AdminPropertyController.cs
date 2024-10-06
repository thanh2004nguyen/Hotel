using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/RoomProperty/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminPropertyController : MyBaseController
    {
		public AdminPropertyController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
        {
            var properties = await _context.RoomProperties.Include(i=>i.IconClass).ToListAsync();

            return View(properties);
        }
        public async Task<IActionResult> Create()
        {
            var icons = await _context.IconClasses.ToListAsync();
            ViewBag.IconList = icons;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoomProperty pro, int? selectedIconId)
        {
            if (ModelState.IsValid)
            {
                pro.IconClassId = selectedIconId;
                _context.Entry(pro).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pro);
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
        public async Task<IActionResult> Edit(RoomProperty property, int? selectedIconId)
        {
            if (ModelState.IsValid)
            {
                property.IconClassId = selectedIconId;
                _context.Entry(property).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var icons = await _context.IconClasses.ToListAsync();
            ViewBag.IconList = icons;
            return View(property);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var property = await _context.RoomProperties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            _context.Entry(property).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
