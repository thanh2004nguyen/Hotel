using Hotel.Data;
using Hotel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("admin/RoomUnity/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminRoomUnityController : MyBaseController
    {
		public AdminRoomUnityController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
		{
			var types = await _context.RoomUnities.ToListAsync();

			return View(types);
		}

		public IActionResult Create()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(RoomUnity unity)
		{
			if (ModelState.IsValid)
			{
				_context.Entry(unity).State = EntityState.Added;
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			else
			{
				return View("Create", unity);
			}
		}

		public async Task<IActionResult> Delete(int id)
		{
			var uni = await _context.RoomUnities.SingleOrDefaultAsync(a=>a.Id ==id);

			Console.WriteLine($"Uniiiiiiiiiiiiiiiiiiiiii:{uni}");
			_context.Entry(uni).State = EntityState.Deleted;
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Edit(int id)
		{

			var uni = await _context.RoomUnities.SingleOrDefaultAsync(a=>a.Id ==id);
			return View(uni);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(RoomUnity unity)
		{
			if (ModelState.IsValid)
            {
                _context.Entry(unity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Redirect("Index");

            }
			else
			{
				var U = await _context.RoomUnities.AsNoTracking().SingleOrDefaultAsync(a => a.Id == unity.Id);
				if (unity.Name == U.Name)
				{
					return Redirect("Index");

				}
				return View(unity);
			}



		}
	}
}
