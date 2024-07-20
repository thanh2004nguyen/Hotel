using Hotel.Data;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
	[Route("admin/Room/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminRoomController : MyBaseController
	{
		public AdminRoomController(HotelDbContext context) : base(context)
		{
		}

		public async Task<IActionResult> Index()
		{
			var rooms = await _context.Rooms
				.Include(r => r.Unities)
				.Include(r => r.RoomType)
				.ToListAsync();


			var roomProperties = await _context.RoomProperties
				.Include(a => a.Details)
				.ToListAsync();

			ViewBag.RoomPropertyList = new SelectList(roomProperties, "Id", "Name");

			return View(rooms);
		}
		public async Task<IActionResult> Create()
		{
			var room = new Room
			{
				Unities = _context.RoomUnities
					.Where(a => a.RoomId == null)
					.ToList(),

				roomProperties = _context.RoomProperties.ToList()

			};
			ViewBag.RoomTypeId = new SelectList(_context.RoomTypes, "Id", "Type");

			return View(room);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Room ro, List<int> Unities, List<IFormFile> ImageFile, List<string> PropertyDetail, List<int> PropertyId)
		{

			if (ModelState.IsValid)
			{
				// add new room
				_context.Rooms.Add(ro);
				await _context.SaveChangesAsync();


				// add Property Detail
				for (int i = 0; i < PropertyDetail.Count; i++)
				{
					var newPropertyDetail = new RoomPropertyDetail
					{
						RoomPropertyId = PropertyId[i],
						Detail = PropertyDetail[i],
						RoomId = ro.Id
					};
					_context.RoomPropertyDetails.Add(newPropertyDetail);

				}
				await _context.SaveChangesAsync();

				string filename = string.Empty;
				// kiem tra xem hinh co duoc upload len hay khong
				foreach (var imgfile in ImageFile)

				{

					if (imgfile != null)
					{
						filename = await CommonMethod.uploadImage(imgfile);
						if (filename != "false")
						{
							var newImage = new Image
							{
								Url = filename,
								RoomId = ro.Id
							};
							_context.Images.Add(newImage);
							await _context.SaveChangesAsync();
						}
						else
						{
							return View();
						}
					}
				}

				//LIst danh sach Unity Selected
				var selectedUnity = await _context.RoomUnities
					 .Where(u => Unities.Contains(u.Id))
					 .ToListAsync();
				// Tao New Unity have RoomID = Created Room (ro)
				foreach (var u in selectedUnity)
				{
					var newUnity = new RoomUnity
					{
						Name = u.Name,
						RoomId = ro.Id
					};
					_context.RoomUnities.Add(newUnity);
				}
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}
			return View(ro);
		}


		public async Task<IActionResult> Edit(int id)
		{
			var room = await _context.Rooms
				.Include(a => a.Unities)
				.Include(b => b.RoomType)
				.SingleOrDefaultAsync(a => a.Id == id);

			var roomProperties = await _context.RoomProperties
			 .Include(a => a.Details)
			 .ToListAsync();

			ViewBag.RoomPropertyList = new SelectList(roomProperties, "Id", "Name");

			var propertyDetails = await _context.RoomPropertyDetails
				   .Where(d => d.RoomId == id)
				   .ToListAsync();

			ViewBag.PropertyDetails = propertyDetails;
			ViewBag.RoomTypeList = new SelectList(_context.RoomTypes.ToList(), "Id", "Type");
			ViewBag.RoomUnitiesList = new SelectList(_context.RoomUnities.Where(a => a.RoomId == null).ToList(), "Id", "Name");
			return View(room);
		}

		[HttpPost]

		public async Task<IActionResult> Edit(Room ro, List<int> Unities, List<IFormFile> ImageFile, List<string> PropertyDetail, List<int> PropertyId)

		{
			if (ModelState.IsValid)
			{
				//adit
				_context.Entry(ro).State = EntityState.Modified;
				await _context.SaveChangesAsync();


				// Edit Property Detail
				for (int i = 0; i < PropertyDetail.Count; i++)
				{
					var Prodetail = await _context.RoomPropertyDetails
						.Where(a => a.RoomId == ro.Id)
						.Where(a => a.RoomPropertyId == PropertyId[i])
						.SingleOrDefaultAsync();

					Prodetail.Detail = PropertyDetail[i];

					Console.WriteLine($"idddddddddddddddddd:{Prodetail.RoomId}");
					_context.Entry(Prodetail).State = EntityState.Modified;
					await _context.SaveChangesAsync();
				}


				// Edit Images
				// Xoa image Cu
				if (ImageFile.Count > 0)
				{
					var oldImg = await _context.Images
				   .Where(a => a.RoomId == ro.Id)
				   .ToListAsync();

					foreach (var img in oldImg)
					{
						_context.Entry(img).State = EntityState.Deleted;
						await _context.SaveChangesAsync();
					}
				}

				string filename = string.Empty;
				foreach (var imgfile in ImageFile)
				{
					if (imgfile != null)
					{
						filename = await CommonMethod.uploadImage(imgfile);
						if (filename != "false")
						{
							var newImage = new Image
							{
								Url = filename,
								RoomId = ro.Id
							};
							_context.Images.Add(newImage);
							await _context.SaveChangesAsync();
						}
						else
						{
							return View();
						}
					}
				}

				var oldUnity = await _context.RoomUnities
				   .Where(a => a.RoomId == ro.Id)
				   .ToListAsync();

				for (int i = 0; i < oldUnity.Count; i++)
				{
					_context.Entry(oldUnity[i]).State = EntityState.Deleted;
					await _context.SaveChangesAsync();

				}

				// Edit Unity
				//LIst danh sach Unity Selected
				var selectedUnity = await _context.RoomUnities
					 .Where(u => Unities.Contains(u.Id))
					 .ToListAsync();
				// Tao New Unity have RoomID = Created Room (ro)
				foreach (var u in selectedUnity)
				{


					var newUnity = new RoomUnity
					{
						Name = u.Name,
						RoomId = ro.Id
					};
					_context.Entry(newUnity).State = EntityState.Added;
				}
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(ro);

		}

		[HttpGet()]
		public async Task<IActionResult> SetRoomStament(int id)
		{
			var data =await _context.Rooms.FindAsync(id);
			if(data == null)
			{
				ViewData["error"] = "Có lỗi xảy ra vui lòng thử lại sau";
				return RedirectToAction("Index");
			}
			else
			{
				data.IsFulled = data.IsFulled == true ? false : true; 
				
				await _context.SaveChangesAsync();
				ViewData["success"] = "Tạng thái phòng đã update thành công";
				return RedirectToAction("Index");

			}
		}

	




	}
}
