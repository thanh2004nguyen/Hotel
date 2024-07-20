using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
	[Route("admin/hotel/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminHotelController : MyBaseController
	{
		public AdminHotelController(HotelDbContext context) : base(context)
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
			var data = await _context.hotelDatas.ToListAsync();
			return View(data);
		}

		public IActionResult Create()
		{
			return View("Create");
		}

		[HttpPost]
		public async Task<IActionResult> Create(HomeDataDto data)
		{
			if (ModelState.IsValid)
			{
				var result = await CommonMethod.uploadImage(data.Image);
				if (result == "false")
				{
					ViewData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
					return View("Create");
				}
				else
				{
					await _context.AddAsync(new HotelData(data, result));
					await _context.SaveChangesAsync();
					TempData["success"] = "Nội dung đã được thêm thành công";
					return RedirectToAction("Index");
				}
			}
			return View("Create");
		}
		public async Task<IActionResult> Edit(int id)
		{
			var data = await _context.hotelDatas.FindAsync(id);
			if (data == null)
			{
				TempData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
				return RedirectToAction("Index");
			}
			else
			{
				return View("Edit", data);
			}
		}
		[HttpPost]
		public async Task<IActionResult> Edit(IFormFile? img, HotelData data)
			
		{
			if (ModelState.IsValid)
			{
				var check = await _context.hotelDatas.FindAsync(data.Id);
				if (check == null)
				{
					ViewData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
					return View("Edit");
				}
				else
				{
					if (img == null)
					{
						check.Content=data.Content;
						check.Header=data.Header;
						check.Type=data.Type;
						await _context.SaveChangesAsync();
                        TempData["success"] = "Nội dung đã được cập nhâp thành công";
                        return RedirectToAction("Index");
                    }
					else {
						var result = await CommonMethod.uploadImage(img);
						if (result == "false")
						{
                            ViewData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
                            return View("Edit");
						}
						else
						{
							check.Header = data.Header;
							check.Content=data.Content;
							check.Url = result;
							check.Type = data.Type;
							await _context.SaveChangesAsync();
                            TempData["success"] = "Nội dung đã được cập nhâp thành công";
                            return RedirectToAction("Index");
						}
					}


				}
				
			}
			
            return View("Edit");
        }

		public async Task<IActionResult> Delete(int id)
		{
			var data= await _context.hotelDatas.FindAsync(id);
			if (data == null)
			{
                TempData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
                return RedirectToAction("Index");
			}
			else
			{
				_context.Entry(data).State = EntityState.Deleted;
				await _context.SaveChangesAsync();
                TempData["error"] = "Nội dung đã được xóa thành công";
                return RedirectToAction("Index");
			}
		}
	}

}
