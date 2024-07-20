using Hotel.Data;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
	[Route("admin/image/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminGaleryController : MyBaseController
    {
		public AdminGaleryController(HotelDbContext context) : base(context)
		{
		}

		public async Task <IActionResult> Index()
        {

            string? success = TempData["success"] as string;

            if (success != null)
            {
                ViewData["ChangePassSuccessed"] = success;
            }

            string? error = TempData["error"] as string;

            if (error != null)
            {
                ViewData["ChangePassSuccessed"] = error;
            }
            var data=await _context.Images.ToListAsync();
            foreach(var i in data)

            {
                var id=_context.Rooms.Where(r=>r.Id==i.RoomId).FirstOrDefault();
                var check = _context.RoomTypes.Where(r => r.Id == id.RoomTypeID).FirstOrDefault();
                i.RoomType = check.Type;
            }
            return View(data);
        }

        public async  Task<IActionResult> Create ()

        {
            var data = await _context.Rooms
                .Include(r=>r.RoomType)
                .ToListAsync();
            ViewData["RoomList"] = data;

            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Image image)
        {
            foreach (var img in image.ImageFile)
            {
                var result = await CommonMethod.uploadImage(img);
                if (result == "false")
                {
                    ViewData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
                    return View("Create");
                }
                else
                {
                    await _context.AddAsync(new Image() { Url=result,RoomId=image.RoomId});
                    await _context.SaveChangesAsync();

                }
            }
            TempData["success"] = "Ảnh đã  đã được thêm thành công";
            return RedirectToAction("Index");


        }

        public async Task<IActionResult> Delete(int id)
        {
            var check = await _context.Images.FindAsync(id);
            if (check == null)
            {
                TempData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
                return RedirectToAction("Index");
            }
            else
            {
                _context.Entry(check).State = EntityState.Deleted;
               await _context.SaveChangesAsync();
                TempData["success"] = "Ảnh đã  đã được xóa thành công";
                return RedirectToAction("Index");
            }

        }
    }
}
