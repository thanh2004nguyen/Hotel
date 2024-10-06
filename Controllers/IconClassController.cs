using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/icon/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class IconClassController : MyBaseController
    {
        public IconClassController(HotelDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _context.IconClasses.ToListAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var dto = new IconCreateDTO();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IconCreateDTO dto)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem icon đã tồn tại
                var existingIcon = await _context.IconClasses.FirstOrDefaultAsync(i => i.Icon == dto.Icon);
                if (existingIcon != null)
                {
                    TempData["ErrorMessage"] = "Icon đã tồn tại";
                    return View(dto);
                }

                var iconClass = new IconClass(dto);

                await _context.IconClasses.AddAsync(iconClass);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm biểu tượng thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

    }
}
