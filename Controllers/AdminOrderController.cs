using Hotel.Data;
using Hotel.Dtos;
using Hotel.Migrations;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Hotel.Controllers
{

	[Route("admin/order/{action}")]
	public class AdminOrderController : MyBaseController
	{
		public AdminOrderController(HotelDbContext context) : base(context)
		{
		}
        
		
		public  IActionResult Index()
        {
            var data = from o in _context.Orders
                       join r in _context.Rooms
                       on o.RoomId equals r.Id into result
                       from r in result.DefaultIfEmpty()
                       select new OrderViewDto
                       {
                           Name = o.Name,
                           Phone = o.Phone,
                           RoomType = r.RoomType!.Type, 
                           Message = o.Message,
                           Date = o.CreatedDate,
                           CheckInDate=o.DayCheckin,
                           OrderId=o.Id,
                           Isviewed=o.IsViewed,
                           Type=o.Type
                           
                       };
              
      
            return View(data.OrderByDescending(o => o.Date));
            
        }
        [HttpPost]
        public async Task Update(int id)
        {
            var data= await _context.Orders.FindAsync(id);
            data.IsViewed=true;
           await _context.SaveChangesAsync();         
        }

        public async Task<bool> Add(OrderDto data)
        {
            try
            {
                await _context.AddAsync(new Order(data));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           }

        [HttpPost]
        public async Task<IActionResult> SetIsFulled(int id)
        {
            Console.WriteLine("_________________________________________AA");

            // Tìm phòng với id cung cấp
            var order = await _context.Orders.FindAsync(id);
            var room = await _context.Rooms.FindAsync(order.RoomId);
            Console.WriteLine(room);
            if (room != null)
            {
                room.IsFulled = false;
                _context.Rooms.Update(room);

                try
                {
                    await _context.SaveChangesAsync();
                    Console.WriteLine("_________________________________________thành công");
                    TempData["success"] = "Order deleted successfully.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"_________________________________________Lỗi khi lưu thay đổi: {ex.Message}");
                    TempData["error"] = "Có lỗi xảy ra khi xóa đơn hàng. Vui lòng thử lại sau.";
                }
            }
            else
            {
                Console.WriteLine("_________________________________________Phòng không tìm thấy");
                TempData["error"] = "Phòng không tìm thấy.";
            }

            return RedirectToAction("MyOrder");
        }

        public async Task<IActionResult> CountOrder()
        {
            var Number =await _context.Orders.CountAsync();
            return Ok(Number);
        }

        }
    }

