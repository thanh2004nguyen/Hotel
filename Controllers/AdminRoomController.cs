using Hotel.Data;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Hotel.Controllers
{
    [Route("admin/Room/{action}")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminRoomController : MyBaseController
    {
        private readonly ILogger<AdminAmenitiesThemeController> _logger;

        public AdminRoomController(HotelDbContext context, ILogger<AdminAmenitiesThemeController> logger) : base(context)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(r => r.RoomAmenities)
                .Include(r => r.RoomType)
                .Include(i => i.Images)
                .ToListAsync();


            var roomProperties = await _context.RoomProperties.ToListAsync();

            ViewBag.RoomPropertyList = new SelectList(roomProperties, "Id", "Name");

            return View(rooms);
        }

        [HttpGet]
        public IActionResult GetAmenitiesByThemes(string themeIds)
        {
            if (string.IsNullOrEmpty(themeIds))
            {
                _logger.LogWarning("No themeIds were received.");
                return Json(new List<string>());
            }
            var ids = themeIds.Split(',').Where(id => int.TryParse(id, out _)).Select(int.Parse).ToArray();

            if (ids.Length == 0)
            {
                _logger.LogWarning("No valid themeIds were received after parsing.");
                return Json(new List<string>());
            }
            var amenities = _context.Amenities
                .Where(a => a.AmenitiesThemeId.HasValue && ids.Contains(a.AmenitiesThemeId.Value)) // So sánh với int array
                .ToList();

            return Json(amenities);
        }


        public async Task<IActionResult> Create()
        {
            // Load required data for the view
            ViewBag.RoomProperties = await _context.RoomProperties.ToListAsync();
            ViewBag.RoomTypeId = new SelectList(await _context.RoomTypes.ToListAsync(), "Id", "Type");
            ViewBag.AmenitiesThemes = new SelectList(await _context.AmenitiesThemes.ToListAsync(), "Id", "Name"); // Load AmenitiesThemes
            ViewBag.Amenities = await _context.Amenities.ToListAsync(); // Load Amenities
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room, List<int> selectedAmenities, List<IFormFile> ImageFile, List<string> selectedAmenitiesThemes, List<int> PropertyIds, bool Status = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add new room to the database
                    _context.Rooms.Add(room);
                    await _context.SaveChangesAsync();

                    // Xử lý hình ảnh tải lên
                    if (ImageFile != null && ImageFile.Count > 0)
                    {
                        foreach (var imgfile in ImageFile)
                        {
                            if (imgfile.Length > 0) // Kiểm tra xem file có dữ liệu không
                            {
                                string filename = await CommonMethod.uploadImage(imgfile);
                                if (filename != "false")
                                {
                                    var newImage = new Image
                                    {
                                        Url = filename,
                                        RoomId = room.Id
                                    };
                                    _context.Images.Add(newImage);
                                }
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();

                    if (selectedAmenities != null && selectedAmenities.Count > 0)
                    {
                        foreach (var amenityId in selectedAmenities)
                        {
                            var amenity = await _context.Amenities.FindAsync(amenityId);
                            if (amenity != null)
                            {
                                var roomAmenity = new RoomAmenities
                                {
                                    RoomId = room.Id,
                                    AmenitiesId = amenity.Id // Lưu ID của tiện ích
                                };
                                _context.RoomAmenities.Add(roomAmenity);
                            }
                            else
                            {
                                ModelState.AddModelError("", $"Tiện ích với ID {amenityId} không tồn tại.");
                            }
                        }
                        await _context.SaveChangesAsync();
                    }


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during creation: " + ex.Message);
                    ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình tạo mới phòng: " + ex.Message);
                }
            }
            else
            {
                // Nếu ModelState không hợp lệ, log lỗi từng trường
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            // Nếu có lỗi, hiển thị lại form với dữ liệu đã nhập
            ViewBag.RoomProperties = await _context.RoomProperties.ToListAsync();
            ViewBag.RoomTypeId = new SelectList(await _context.RoomTypes.ToListAsync(), "Id", "Type");
            ViewBag.AmenitiesThemes = new SelectList(await _context.AmenitiesThemes.ToListAsync(), "Id", "Name");

            return View(room);
        }
        [HttpPost]
        public JsonResult DeleteRoom(int roomId)
        {
            var room = _context.Rooms.Find(roomId);

            if (room == null)
            {
                return Json(new { success = false, message = "Room not found." });
            }

            _context.Rooms.Remove(room);
            _context.SaveChanges();

            return Json(new { success = true, message = "Room deleted successfully.", roomId = roomId });
        }

        /*  public async Task<IActionResult> Edit(int id)
          {
              var room = await _context.Rooms
                  .Include(a => a.Amenities)
                  .Include(b => b.RoomType)
                  .SingleOrDefaultAsync(a => a.Id == id);

              var roomProperties = await _context.RoomProperties.ToListAsync();
              ViewBag.RoomPropertyList = new SelectList(roomProperties, "Id", "Name");
              ViewBag.RoomTypeList = new SelectList(_context.RoomTypes.ToList(), "Id", "Type");
              ViewBag.AmenitiesList = new SelectList(_context.Amenities.Where(a => a.RoomId == null).ToList(), "Id", "Name");
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

                  var oldUnity = await _context.Amenities
                     .Where(a => a.RoomId == ro.Id)
                     .ToListAsync();

                  for (int i = 0; i < oldUnity.Count; i++)
                  {
                      _context.Entry(oldUnity[i]).State = EntityState.Deleted;
                      await _context.SaveChangesAsync();

                  }

                  // Edit Unity
                  //LIst danh sach Unity Selected
                  var selectedUnity = await _context.Amenities
                       .Where(u => Unities.Contains(u.Id))
                       .ToListAsync();
                  // Tao New Unity have RoomID = Created Room (ro)
                  foreach (var u in selectedUnity)
                  {


                      var newUnity = new Amenities
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

          }*/
    }
}
