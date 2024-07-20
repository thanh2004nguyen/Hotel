using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("admin/banner/{action}")]
    public class AdminBannerController : MyBaseController
    {
		public AdminBannerController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
        {
            var data =await _context.Banners.ToListAsync();
            string messageSuccess = TempData["Success"] as string;
            if (messageSuccess != null)
            {
                ViewData["Success"] = messageSuccess;
            }

            string messageError = TempData["Error"] as string;
            if (messageError != null)
            {
                ViewData["error"] = messageError;
            }

            return View(data);
        }

        public IActionResult Create() {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(BannerDto data)
        {
            if(ModelState.IsValid)
            {
                var image =await CommonMethod.uploadImage(data.image);
                if (image!="false")
                {
                    await _context.AddAsync(new Banner(data, image));
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Tạo Banner  thành công";
                    return RedirectToAction("Index");   
                }
            }
            return View("Create");
            
        }

        public async Task<IActionResult> Update(int id)
        {
            var Banner = await _context.Banners.FindAsync(id);
            if (Banner != null)
            {
                return View("Update", Banner);
            }
            else
            {
                ViewData["Fail"] = "Có lỗi xảy ra trong quá trình sử lý . Vui long thử lại";
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(string content, string title, int id, IFormFile img)
        {
            if (img == null)
            {
                var banner = await _context.Banners.FindAsync(id);
                if (banner != null)
                {
                    banner.Title = title;
                    banner.Content = content;
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Banner cập nhập thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra vui lòng thử lại";
                    return RedirectToAction("Index");

                }

            }
            else
            {
                var NewImage = await CommonMethod.uploadImage(img);
                if (NewImage != "false")
                {
                    var data = await _context.Banners.FindAsync(id);
                    data.Image = NewImage;
                    _context.Entry(data).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Banner cập nhập thành công";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Có lỗi xảy ra vui lòng thử lại";
                    return RedirectToAction("Index");
                }
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
            {
                TempData["Error"] = "Có lỗi xảy ra vui lòng thử lại";
                return RedirectToAction("Index");
            }
            else
            {
                _context.Entry(banner).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Banner đã được xáo thành công";
                return RedirectToAction("Index");
            }

        }

    }
}
