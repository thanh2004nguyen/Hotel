using Hotel.Data;
using Hotel.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Hotel.Models;
using System.Runtime.CompilerServices;

namespace Hotel.Controllers
{
    public class AdminUserManagementController : MyBaseController
    {
        public AdminUserManagementController(HotelDbContext context) : base(context)
        {
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ListAccount()
        {
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var accounts = await _context.Users.Where(u => u.Id != userId).ToListAsync();
            Console.WriteLine("Count account", accounts.Count);
            return View(accounts);
        }
    }
}
