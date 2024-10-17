using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json.Nodes;
using System.Net.Http.Headers; // Cần thêm cho việc xử lý header
using PayPalCheckoutSdk.Orders; // Cần thêm cho việc tạo đơn hàng (nếu dùng SDK)
using System.Text.Json;
using Hotel.Filters;
using System.Security.Claims; // Cho việc xử lý JSON

namespace Hotel.Controllers
{
    [AllowAnonymous]
    public class RoomController : MyBaseController
    {
        private readonly PaypalClient _paypalClient;
        public RoomController(HotelDbContext context, PaypalClient paypalClient) : base(context)
        {
            _paypalClient = paypalClient;
        }

        /* Handle controller view booking */

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, FilterRoomData filterData = null)
        {
            // Start with the base query to get all rooms
            var query = _context.Rooms
                .Include(r => r.RoomAmenities)
                    .ThenInclude(t => t.Amenities)
                        .ThenInclude(a => a.AmenitiesTheme)
                            .ThenInclude(i => i.IconClass)
                .Include(r => r.RoomType)
                .Include(r => r.Policy)
                .Include(i => i.Images)
                .AsQueryable();

            // Apply filtering logic only if filterData is provided
            if (filterData != null)
            {
                if (filterData.MaxPrice > 0)
                {
                    query = query.Where(r => r.Price <= filterData.MaxPrice && r.Price >= filterData.MinPrice);
                }
                if (filterData.MinRating > 0)
                {
                    query = query.Where(r => r.Rating >= filterData.MinRating);
                }
                if (filterData.RoomTypes != null && filterData.RoomTypes.Any())
                {
                    query = query.Where(r => filterData.RoomTypes.Contains(r.RoomTypeID));
                }
            }

            // Count total rooms after filtering
            var totalRooms = await query.CountAsync();

            // Apply pagination
            var rooms = await query
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalPages = (int)Math.Ceiling((double)totalRooms / pageSize);
            // Prepare the view model
            return View(new RoomViewDto()
            {
                ListRooms = rooms,
                ListPropertys = await _context.RoomProperties.ToListAsync(),
                CurrentPage = page,
                TotalPages = totalPages,
                RoomTypeCounts = await _context.Rooms
                    .GroupBy(r => new { r.RoomTypeID, r.RoomType.Type })
                    .Select(g => new RoomTypeCountDto
                    {
                        RoomTypeId = g.Key.RoomTypeID,
                        RoomTypeName = g.Key.Type,
                        RoomCount = g.Count()
                    })
                    .ToListAsync(),
                roomsCount = totalRooms,
                FilterRoomData = filterData
            });
        }

        [HttpGet]
        public async Task<IActionResult> Booking(int id)
        {
            var room = await _context.Rooms
            .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amenities)
                    .ThenInclude(a => a.IconClass)
            .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amenities.AmenitiesTheme)
                    .ThenInclude(at => at.IconClass)
            .Include(r => r.RoomType)
            .Include(r => r.Policy)
            .Include(r => r.Images)
            .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            // Tìm danh sách các phòng liên quan
            var relatedRooms = await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.RoomWithRoomProperties)
                    .ThenInclude(rp => rp.RoomProperty)
                         .ThenInclude(icon => icon!.IconClass)
                .Where(r => r.RoomTypeID == room.RoomTypeID && r.Id != room.Id)
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.Amenities.AmenitiesTheme)
                        .ThenInclude(at => at.IconClass)
                .Take(5)
                .ToListAsync();

            ViewBag.RelatedRooms = relatedRooms;

            return View(room);
        }




        [HttpGet]
        public async Task<IActionResult> AddBooking(int id)
        {
            var room = await _context.Rooms
            .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amenities)
                    .ThenInclude(a => a.IconClass)
            .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.Amenities.AmenitiesTheme)
                    .ThenInclude(at => at.IconClass)
            .Include(r => r.RoomWithRoomProperties)
            .ThenInclude(p => p.RoomProperty)
            .Include(r => r.RoomType)
            .Include(r => r.Policy)
            .Include(r => r.Images)
            .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }
            ViewBag.ClientId = _paypalClient.ClientId;
            Console.WriteLine("CLIENTID: ", _paypalClient.ClientId);
            return View(room);
        }


        // Action tạo đơn hàng PayPal
        [HttpPost]
        public async Task<IActionResult> Order([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            try
            {
                // Thực hiện các bước để tạo đơn hàng
                var price = orderRequest.Price; // Giá tiền
                var currency = "USD"; // Tiền tệ
                var reference = GetRandomInvoiceNumber();
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value; 
                int userId = 0; // Khởi tạo biến userId
                if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out userId))
                {
                    // Chuyển đổi thành công
                }
                else
                {
                    return BadRequest("User ID không hợp lệ.");
                }
                var response = await _paypalClient.CreateOrder(price.ToString(), currency, reference);

                // Tạo booking
                var booking = new Booking
                {
                    CustomerName = orderRequest.CustomerName,
                    CustomerEmail = orderRequest.CustomerEmail,
                    CustomerPhone = orderRequest.CustomerPhone,
                    SmokingPreference = orderRequest.SmokingPreference,
                    BedPreference = orderRequest.BedPreference,
                    SpecialRequest = orderRequest.SpecialRequest,
                    Price = orderRequest.Price,
                    CheckinDate = orderRequest.CheckinDate,
                    CheckoutDate = orderRequest.CheckoutDate,
                    TotalNights = orderRequest.TotalNights,
                    RoomId = orderRequest.RoomId,
                    UserId = userId
                };

                // Lưu booking vào cơ sở dữ liệu
                await _context.Bookings.AddAsync(booking, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                var bookingId = booking.Id;
                Console.WriteLine(bookingId);
                // Trả về response bao gồm order id và booking id
                return Ok(new { id = response.id, bookingId }); // booking.Id là ID của booking vừa tạo
            }
            catch (Exception e)
            {
                var error = new { e.GetBaseException().Message };
                return BadRequest(error);
            }
        }


        // Phương thức Capture thanh toán
        public async Task<IActionResult> Capture(string orderId, int bookingId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                // Kiểm tra phản hồi và lấy giá trị thanh toán
                var amount = response.purchase_units.FirstOrDefault()?.payments.captures.FirstOrDefault()?.amount?.value;

                // Tạo đối tượng Payment và lưu vào cơ sở dữ liệu
                var payment = new Payment
                {
                    PaymentMethod = "PayPal",
                    Amount = decimal.Parse(amount),
                    PaymentDate = DateTime.UtcNow,
                    TransactionId = response.id, // Mã giao dịch
                    BookingId = bookingId // Gán bookingId vào payment
                };

                await _context.Payments.AddAsync(payment, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new { e.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }




        //request view to controller create order 
        public class OrderRequest
        {
            public decimal Price { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }
            public string CustomerPhone { get; set; }
            public string SmokingPreference { get; set; }
            public string BedPreference { get; set; }
            public string SpecialRequest { get; set; }
            public DateTime CheckinDate { get; set; }
            public DateTime CheckoutDate { get; set; }
            public int TotalNights { get; set; }
            public int RoomId { get; set; }
            
        }
    }
}