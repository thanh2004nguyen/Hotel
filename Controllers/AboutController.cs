using Hotel.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [AllowAnonymous]
    public class AboutController : MyBaseController
    {
		public AboutController(HotelDbContext context) : base(context)
		{
		}

	
		public async  Task<IActionResult> Index(string type)
        {
            var data = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .Where(r => r.RoomType.Type.ToLower() == type.ToLower())
                .FirstOrDefaultAsync();

            return View(data);
        }
        [Route("price")]
        public  async Task<IActionResult> Price()
        {
            var data = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.roomProperties)
                .Include(r=>r.Details)
                .ToListAsync();

            foreach(var i in data)
            {
              foreach(var n in i.Details)
                {
                    var m= _context.RoomProperties.Find(n.RoomPropertyId);
                    n.Name = m.Name;
                }
            }
          

            return View("Price",data);
        }

		
	}
}
