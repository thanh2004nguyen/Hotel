using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        [HttpPost]
        public async Task<IActionResult> Like(int commentId, string likeStyle)
        {
            if (likeStyle != "liked" && likeStyle != "unliked")
            {
                return BadRequest(new { message = "Invalid likeStyle parameter" });
            }

            var claims = User.Claims.ToList();
            var userIdClaim = claims.FirstOrDefault(c => c.Type == "id"); // Id user login
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest(new { message = "Invalid user ID format in claims" });
            }

            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound(new { message = "Comment not found" });
            }
            var userCommentLike = await _context.UserCommentLikes
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommentId == commentId);

            if (likeStyle == "liked")
            {
                if (userCommentLike != null)
                {
                    return BadRequest(new { message = "User has already liked this comment" });
                }

                var newUserCommentLike = new UserCommentLike
                {
                    UserId = userId,
                    CommentId = commentId
                };

                await _context.UserCommentLikes.AddAsync(newUserCommentLike);
                comment.like += 1;
            }
            else if (likeStyle == "unliked")
            {
                if (userCommentLike == null)
                {
                    return BadRequest(new { message = "User has not liked this comment yet" });
                }

                _context.UserCommentLikes.Remove(userCommentLike);
                comment.like -= 1;
            }

            await _context.SaveChangesAsync();
            return Json(new { commentId = comment.Id, likes = comment.like , likeStatus = likeStyle });
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
