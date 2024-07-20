using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
	[Route("admin/comment/{action}")]
	[Authorize(Policy = "AdminOnly")]
	public class AdminCommentController : MyBaseController
    {
		public AdminCommentController(HotelDbContext context) : base(context)
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

           
                    
           

            var data = await _context.Comments
                .ToListAsync();
            foreach(var c in data)
            {
               var x = _context.Rooms.Include(d => d.RoomType).Where(r => r.Id == c.RoomId).FirstOrDefault();
                c.RoomType = x.RoomType.Type;
            }
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
             var data= await _context.Rooms
                .Include(d=>d.RoomType)
                .ToListAsync();
            ViewData["PeopleList"] = data;
            return View("Create");
        }

        [HttpPost]
       public  async Task<IActionResult> Create(CommentDto data)
        {
           
           if(ModelState.IsValid) {
                var image = await CommonMethod.uploadImage(data.image);
                if (image == "false")
                {
                    ViewData["error"] = "có lỗi xảy ra vui lòng thử lại sau";
                
                    return View("Create");
                }
                else
                {
                    var item = new Comment()
                    {
                        Name = data.Name,
                        Content = data.Content,
                        avatar = image,
                        RoomId = data.RoomId,
                         start = data.start>5?5:data.start

                    };
                    await _context.AddAsync(item);
                    await _context.SaveChangesAsync();
					TempData["success"] = "Comment đã được thêm thành công";
                   
                    return RedirectToAction("Index");
                }
            }
          
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
           var check= await _context.Comments.FindAsync(id); 
            if(check== null)
            {
				TempData["error"] = "có lỗi xảy ra vui lòng thử lại sau";
				return RedirectToAction("Index");
            }
            else
            {
                _context.Remove(check);
              await  _context.SaveChangesAsync();
				TempData["success"] = "có lỗi xảy ra vui lòng thử lại sau";
				return RedirectToAction("Index");
            }

        }
    }
}
