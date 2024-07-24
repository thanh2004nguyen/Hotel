using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel.Controllers
{
    [Route("SingleRoom/{action}")]
    [AllowAnonymous]
    public class SingleRoomController : MyBaseController
    {
		public SingleRoomController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index(int id)
        {
            string? message = TempData["success"] as string;
            if (message != null)
            {
                ViewData["success"] = message;
            }




            string? error = TempData["error"] as string;
            if (error != null)
            {
                ViewData["error"] = error;
            }

            var comments = await _context.Comments.Where(c => c.RoomId == id).ToListAsync();

            var room = await _context.Rooms
                .Include(a => a.Unities)
                .Include(b => b.roomProperties)
                .Include(c => c.Images)
                .Include(d => d.Details)
                .Include(e => e.RoomType)
                .SingleOrDefaultAsync(x => x.Id == id);

            var policies = await _context.RoomPolicies.ToListAsync();

            
            
                foreach (var detail in room.Details)
                {
                    var s = await _context.RoomProperties.FindAsync(detail.RoomPropertyId);
                    detail.Name = s.Name;
                }
            


            return View(new RoomDto()
            {
                Room=room,
                Policies=policies,
                Comments=comments

            });
        }

        public async Task<IActionResult> HandleAddOrder(RoomDto data,int id) 
        {

            if (ModelState.IsValid)
            {
                AdminOrderController action = new AdminOrderController(_context);
                var result =await action.Add(data.Order);
                if (result == true)
                {
                    TempData["success"] = "Thông tin đã được gửi đi thành công. Nhân viên sẽ liên hệ để xác nhận thông tin sớm nhất có thể.Xin cảm ơn !";
                    var room = await _context.Rooms.SingleOrDefaultAsync(r=>r.Id==id);
                    if (room != null)
                    {
                        room.IsFulled = true;
                        _context.Rooms.Update(room);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToAction("Index", new { id = id });
                }
                else
                {
                    TempData["error"] = "Có lỗi xảy ra vui lòng. nhập lại sau";
                    return RedirectToAction("Index", new { id=id});
                }

            }
            // Ghi các lỗi vào console để kiểm tra
            foreach (var modelState in ModelState)
            {
                var errors = modelState.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error in {modelState.Key}: {error.ErrorMessage}");
                }
            }
            TempData["error"] = "Vui lòng nhập đủ tên và địa chỉ";
            return RedirectToAction("Index", new { id = id });

        }

        public async Task<IActionResult> MyOrder(int id)
        {
            // Assuming User is authenticated and has an ID
            var claims = User.Claims.ToList();
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "id"); // Id user login
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID format in claims" });
            }

            // Get all orders for the current user, including Room and Room.Images
            var userOrders = await _context.Orders
                .Include(o => o.Room).ThenInclude(r => r.RoomType)
                .Include(o => o.Room).ThenInclude(r => r.Details)
                .Include(o => o.Room).ThenInclude(r => r.Images ) // Bao gồm hình ảnh liên kết với phòng
                .Where(o => o.UserId == userId)
                .ToListAsync();
            // Check if data is loaded and print details to the console
            
        
                    // Kiểm tra dữ liệu và xử lý lỗi nếu không có đơn hàng
            if (userOrders == null || !userOrders.Any())
            {
                return View();
            }

            // Trả về view với dữ liệu đơn hàng
            return View(userOrders);
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int id)
        {
            Console.WriteLine("_________________________________________AA");
            // Assuming User is authenticated and has an ID
            var claims = User.Claims.ToList();
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "id"); // Id user login
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID format in claims" });
            }

            // Find the order to delete
            var order = await _context.Orders
                .Include(o => o.Room)
                .SingleOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found or does not belong to the user" });
            }

            var room = order.Room;

            // Remove the order
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            if (room != null)
            {
                room.IsFulled = false;
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Order deleted successfully.";
            return RedirectToAction("MyOrder");
        }
    }
}
