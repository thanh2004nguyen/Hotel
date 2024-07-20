using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers

{
    [Route("admin/RoomType/{action}")]
    [Authorize(Policy = "AdminOnly")]

    public class AdminRoomTypeController : MyBaseController
    {
		public AdminRoomTypeController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
        {
            var types = await _context.RoomTypes.ToListAsync();

            return View(types);
        }
      
        public  IActionResult Create()
        {

            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoomType room)
        {
            if(ModelState.IsValid) 
            {
                _context.Entry<RoomType>(room).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View("Create", room);

            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.RoomTypes.SingleOrDefaultAsync(a => a.Id == id);
            var room = await _context.Rooms
                .Where(r=>r.RoomType.Id == id)
                .ToListAsync();
            if(room.Count==0)
            {
                _context.Entry(type!).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể xóa vì có tồn tại phòng trong Loại Phòng này!";
                return RedirectToAction("Index");
            }
          
        }

        public async Task<IActionResult> Edit(int id)
        {
            var type = await _context.RoomTypes.SingleOrDefaultAsync(a => a.Id == id);
            return View(type);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoomType Rtype)
        {
           
            if (ModelState.IsValid) 
            {

               var data= await _context.RoomTypes.FindAsync(Rtype.Id);
                data.Description = Rtype.Description;
                data.Type = Rtype.Type;
                await _context.SaveChangesAsync();
                return Redirect("Index");

            }

            return RedirectToAction("Edit", new { id=Rtype.Id});

        }

    }
}
