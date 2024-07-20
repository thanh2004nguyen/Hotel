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
            var properties = await _context.RoomProperties.ToListAsync();

            return View(properties);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoomProperty pro)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(pro).State = EntityState.Added;
                await _context.SaveChangesAsync();

                var rooms = await _context.Rooms.ToListAsync();
                foreach (var r in rooms)
                {
                    var prodetail = new RoomPropertyDetail
                    {
                        Detail = "NO DATA",
                        RoomPropertyId = pro.Id,
                        RoomId = r.Id

                    };

                    _context.Entry(prodetail).State = EntityState.Added;
                    await _context.SaveChangesAsync();

                }
                return RedirectToAction("Index");
            }
            return View(pro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pro = await _context.RoomProperties.FindAsync(id);

            _context.Entry(pro).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            var type = await _context.RoomProperties.SingleOrDefaultAsync(a => a.Id == id);
            return View(type);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomProperty Rtype)
        {
            if(ModelState.IsValid) 
            {
                _context.Entry(Rtype).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Redirect("Index");

            }

            else
            {
                var U = await _context.RoomProperties.AsNoTracking().SingleOrDefaultAsync(a => a.Id == Rtype.Id);
                if (Rtype.Name == U.Name)
                {
                    return Redirect("Index");

                }
                return View(Rtype);
            }
        }


    }
}
