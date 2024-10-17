namespace Hotel.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using System.Linq;
    using Hotel.Models;
    using PayPalCheckoutSdk.Orders;
    using Hotel.Data;
    using Microsoft.AspNetCore.Authorization;
    using Hotel.Dtos;

	public class BookingController : Controller
	{
		private readonly HotelDbContext _context; // DbContext của bạn

		public BookingController(HotelDbContext context)
		{
			_context = context;
		}

		// Phương thức để hiển thị danh sách đơn đặt phòng và thanh toán
		public async Task<IActionResult> Index()
		{
			var userIdClaim = User.FindFirst("id");
			if (userIdClaim == null)
			{
				return Unauthorized();
			}
			int userId = int.Parse(userIdClaim.Value);

			// Lấy danh sách bookings và payments
			var bookingsWithPayments = await (from b in _context.Bookings
											  where b.UserId == userId
											  join p in _context.Payments on b.Id equals p.BookingId into payments
											  select new
											  {
												  Booking = b,
												  Payments = payments.ToList() // Chuyển payments thành danh sách
											  }).ToListAsync();
			return View(bookingsWithPayments);

		}


		public async Task<IActionResult> Details(int id)
		{
			var bookingWithDetails = await (from b in _context.Bookings
											 .Include(b => b.Room) // Bao gồm thông tin phòng
											where b.Id == id
											join p in _context.Payments on b.Id equals p.BookingId into payments
											select new
											{
												Booking = b,
												Payments = payments.ToList() // Chuyển payments thành danh sách
											}).FirstOrDefaultAsync();

			if (bookingWithDetails == null)
			{
				return NotFound();
			}

			return View(bookingWithDetails);
		}


	}
}
