using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [AllowAnonymous]
    public class RoomController : MyBaseController
    {
		public RoomController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
        {

            var rooms = await _context.Rooms
                .Include(a => a.RoomType)
                .Include(a => a.Images!.Take(1))
                .ToListAsync();


            var RoomPro = await _context.RoomProperties
                 .Include(a => a.Details)
                 .ToListAsync();

            return View(new RoomViewDto()
            {
                ListRoom = rooms,
                ListProperty = RoomPro
            });

        }

    }
}
