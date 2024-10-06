using Hotel.Data;
using Hotel.Dtos;
using Hotel.Models;
using Hotel.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Controllers
{
    [Route("dashboard")]
    [Authorize(Policy = "AdminOnly")]
    public class AdminDashboardController : MyBaseController
    {
        public AdminDashboardController(HotelDbContext context) : base(context)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Trả về trang dashboard admin
            return View();
        }
    }
}
