using Hotel.Data;
using Hotel.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers.Api
{

	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly HotelDbContext _context;

		public ChatController(HotelDbContext context)
		{
			_context = context;
		}

		// GET: api/messages/{userId}
		[HttpGet("/api/messages/{userId}")]
		public async Task<IActionResult> GetMessagesByUserId(int userId)
		{
			try
			{
				Console.WriteLine("ĐÃ VÀO CONTROLLER >");
				// Lấy danh sách tin nhắn gửi hoặc nhận bởi userId
				var messages = await _context.Messages
					.Where(m => m.SenderId == userId || m.ReceiverId == userId)
					.OrderBy(m => m.DateSend)
					.Take(100)
					.Select(m => new MessageDto
					{
						MessageId = m.MessageId,
						SenderId = m.SenderId.ToString(),
						ReceiverId = m.ReceiverId.ToString(),
						Content = m.Content,
						DateSend = m.DateSend
					})
					.ToListAsync();
				if (messages == null || messages.Count == 0)
				{
					return NotFound("No messages found for this user.");
				}

				return Ok(messages);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

        // GET: api/messages/{employeeId}/{userId}
        [HttpGet("/api/messages/{employeeId}/{userId}")]
        public async Task<IActionResult> GetMessagesEmpWithUser(int employeeId, int userId)
        {
            try
            {
                Console.WriteLine("ĐÃ VÀO CONTROLLER Emp>");
                // Lấy danh sách tin nhắn gửi hoặc nhận bởi userId
                var messages = await _context.Messages
                    .Where(m => (m.SenderId == employeeId && m.ReceiverId == userId) ||
                        (m.SenderId == userId && m.ReceiverId == employeeId))
                    .OrderBy(m => m.DateSend)
                    .Take(100)
                    .Select(m => new MessageDto
                    {
                        MessageId = m.MessageId,
                        SenderId = m.SenderId.ToString(),
                        ReceiverId = m.ReceiverId.ToString(),
                        Content = m.Content,
                        DateSend = m.DateSend
                    })
                    .ToListAsync();
                if (messages == null || messages.Count == 0)
                {
                    return NotFound("No messages found for this user.");
                }

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("/api/users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(u => u.Role == "user")
                    .Select(m => new UserDto
                    {
                        Id = m.Id,
                        Username = m.Username,
                        IsOnline = m.IsOnline
                    })
                    .ToListAsync();
				Console.WriteLine($"{users.Count} users", users);
                if (users == null || users.Count == 0)
                {
                    return NotFound("No users found.");
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}