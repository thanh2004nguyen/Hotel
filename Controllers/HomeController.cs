using Hotel.Data;
using Hotel.Dtos;
using Hotel.Filters;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics;

namespace Hotel.Controllers
{
    [AllowAnonymous]
	public class HomeController : MyBaseController
    {
		public HomeController(HotelDbContext context) : base(context)
		{
		}
       
		public async Task<IActionResult> Index()
        {
            var banner = await _context.Banners.ToListAsync();
            var comment= await _context.Comments
                .Take(5)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
            var rooms = await _context.Rooms
                .Include(r => r.RoomAmenities)
                    .ThenInclude(a=>a.Amenities)
                        .ThenInclude(b=>b.AmenitiesTheme)
                            .ThenInclude(at=>at.IconClass)
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .Include(r => r.RoomWithRoomProperties)
                    .ThenInclude(rp => rp.RoomProperty)
                .ToListAsync();


            var iconClasses = await _context.IconClasses.ToListAsync();
            var hotel= await _context.hotelDatas
                .ToListAsync();
            return View(new HomeDto()
            {
                Banners = banner,
                Rooms = rooms,
                IconClasses = iconClasses,
                hotelDatas = hotel,
                comments=comment
            });

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}