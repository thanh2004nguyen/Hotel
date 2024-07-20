using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/RoomPropertyDetail/{action}")]
    [Authorize(Policy = "AdminOnly")]
    
    public class AdminPropertyDetailController : MyBaseController
    {
		public AdminPropertyDetailController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
        {
            var prodetail = await _context.RoomPropertyDetails.ToListAsync();

            return View(prodetail);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.RoomPropertyDetail = new SelectList(_context.RoomProperties, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomPropertyDetail Prodetail)
        {
            if(ModelState.IsValid) 
            {
                _context.Entry(Prodetail).State = EntityState.Added;
                await _context.SaveChangesAsync();   
                return RedirectToAction("Index");   
            }
            return View(Prodetail);    
        }
    }
}
