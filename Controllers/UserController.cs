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
  
    public class UserController : MyBaseController
    {
		public UserController(HotelDbContext context) : base(context)
		{
		}

		[Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            string? message = TempData["passchange"] as string;

            if (message != null)
            {
                ViewData["ChangePassSuccessed"] = message;
            }

            string? success = TempData["success"] as string;

            if (message != null)
            {
                ViewData["success"] = success;
            }


            return View("Login");
        }

        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto data)
        {
            if (ModelState.IsValid)
            {

                var check = await _context.Users
                    .Where(u => u.Username == data.Email)
                    .FirstOrDefaultAsync();

                if (check == null)
                {
                    ViewData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng vui lòng kiểm tra lại";
                    return View("Login");

                }
                else
                {

                    if (!BCrypt.Net.BCrypt.Verify(data.Password, check.Password))
                    {
                        ViewData["Error"] = "Tên đăng nhập hoặc mật khẩu không đúng vui lòng kiểm tra lại";
                        return View("Login");
                    }

                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,check.Username),
                         new Claim("id", check.Id.ToString()),
                         new Claim(ClaimTypes.Role,check.Role)

                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var properties = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = true
                    };
                    await HttpContext
                        .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                    return RedirectToAction("Index", "Home");
                }

            }
            return View("Login");
        }
        
        [Route("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");

        }

        [Route("changePassword")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        
        [Route("Register")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Register()
        {
           
            return View("Register");
        }
        
        [Route("Register")]
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        
        public async Task<IActionResult> Register(RegisterDto info)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                    Username = info.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(info.Pass),
                    Role = info.Role
                };

                await _context.AddAsync(data);
                await _context.SaveChangesAsync();
                TempData["success"] = "Đăng ký thành công. vui lòng đăng nhập để sử dụng";
                return RedirectToAction("Login");
            }
            return View("Register");
        }
      
        [Route("changePassword")]
        [HttpPost]

        public async Task<IActionResult> ChangePassword(PassDto data)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim != null)
                {
                    string userId = userIdClaim.Value;
                    var check = await _context.Users.FindAsync(int.Parse(userId));
                    if (check == null)
                    {
                        ViewData["Error"] = "Tài khoản hiện tại không tồn tại";
                        return View();
                    }
                    else
                    {
                        if (!BCrypt.Net.BCrypt.Verify(data.CurrentPassword, check.Password))
                        {
                            ViewData["Error"] = "Mật khẩu hiện tại sai, vui lòng kiểm tra lại";
                            return View();
                        }
                        else
                        {
                            var newPassword = BCrypt.Net.BCrypt.HashPassword(data.NewPassword);
                            check.Password = newPassword;
                            await _context.SaveChangesAsync();
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            TempData["passchange"] = "Mật khẩu đã thay đổi thành công, vui đăng nhập lại";
                            return RedirectToAction("Login");
                        }
                    }

                }
                return View("ChangePassword");
            }
            return View("ChangePassword");
        }
        [AllowAnonymous]
        [Route("unauthozied")]
        public IActionResult UnAuthorize()
        {
            return View("unauthorize");
        }

        }
    }