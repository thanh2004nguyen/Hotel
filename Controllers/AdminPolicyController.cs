using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/policy/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminPolicyController : MyBaseController
    {
		public AdminPolicyController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
        {
            string? success = TempData["success"] as string;

            if (success != null)
            {
                ViewData["success"] = success;
            }

            string? error = TempData["error"] as string;

            if (error != null)
            {
                ViewData["error"] = error;
            }
            var data = await _context.RoomPolicies.ToListAsync();

            return View(data);
        }

        public IActionResult Create() {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoomPolicy data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(data).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Policy đã được thêm thành công";
                    return RedirectToAction("Index");
                }
                return View("Create");
            }catch(Exception ex)
            {
                ViewData["success"] = "Có lỗi xảy ra vui lòng thử lại sau";
                return View("Create");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _context.RoomPolicies.FindAsync(id);
            if(data == null)
            {
                ViewData["success"] = "Có lỗi xảy ra vui lòng thử lại sau";
                return View("Index");
            }
            else
            {
                return View("Edit",data);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoomPolicy data)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(data).State= EntityState.Modified;
              await  _context.SaveChangesAsync();
                TempData["success"] = "Policy đã được thay đổi thành công";
                return RedirectToAction("Index");
            }
            return View("Create");
        }
        public async Task<IActionResult>Delete(int id)
        {
            var data =await _context.RoomPolicies.FindAsync(id);
            if (data == null)
            {
                ViewData["success"] = "Có lỗi xảy ra vui lòng thử lại sau";
                return View("Index");
            }
            else
            {
                _context.Entry(data).State=EntityState.Deleted;
                await _context.SaveChangesAsync();
                TempData["success"] = "Policy đã được xóa thành công";
                return RedirectToAction("Index");

            }
        }
    }
}
