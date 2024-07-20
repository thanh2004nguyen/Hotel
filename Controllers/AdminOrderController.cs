using Hotel.Data;
using Hotel.Dtos;
using Hotel.Migrations;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> CountOrder()
        {
            var Number =await _context.Orders.CountAsync();
            return Ok(Number);
        }

        }
    }

