using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("dashboard")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminDashboardController : MyBaseController
    {
        public AdminDashboardController(HotelDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
			// Trả về trang dashboard admin
			var userIdClaim = User.FindFirst("id");
			if (userIdClaim == null)
			{
				return Unauthorized();
			}
			int userId = int.Parse(userIdClaim.Value);

			// Lấy danh sách bookings
			#pragma warning disable CS8604 // Possible null reference argument.
			var bookingsWithPayments = await(from b in _context.Bookings
											  .Include(b => b.Room)
											 .ThenInclude(r => r.Images)
											 where b.UserId == userId
                                             orderby b.CreatedDate descending
                                             join p in _context.Payments on b.Id equals p.BookingId into payments
											 let room = b.Room
											 select new
											 {
												 Booking = b,
												 Payments = payments.ToList(),
												 FirstImage = room.Images.FirstOrDefault()
											 })
											 .Take(6).ToListAsync();
			#pragma warning restore CS8604 // Possible null reference argument.
			return View(bookingsWithPayments);
        }
    }
}
