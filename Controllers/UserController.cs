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
using static MessagesConstant;
using Hotel.ServiceImql;

namespace Hotel.Controllers
{

    public class UserController : MyBaseController
    {
        private readonly ILoginService _loginService;
        public UserController(HotelDbContext context, ILoginService service) : base(context)
		{
            _loginService = service;
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
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto data)
        {
            Models.User? user = null;
            if (ModelState.IsValid)
            {
                //Xác thực email + password → trả về token
                var token = await _loginService.AuthenticateAsync(data.Email, data.Password);

                //Kiểm tra token có phải là JWT không (ít nhất có 2 dấu chấm)
                if (string.IsNullOrEmpty(token) || !token.Contains("."))
                {
                    ViewData["Error"] = MessagesConstant.User.InvalidLogin;
                    return View("Login");
                }

                //Lấy thông tin người dùng để lưu vào Claims, Session
                user = await _context.Users
                                  .Where(u => u.Email == data.Email)
                                  .FirstOrDefaultAsync();
                Console.WriteLine(user);
                var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? ""),
                    new Claim("token", token)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var properties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                                          new ClaimsPrincipal(claimsIdentity), properties);
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.Username ?? "");
                HttpContext.Session.SetString("Email", user.Email ?? "");
                HttpContext.Session.SetString("UserRole", user.Role ?? "");
            }
            return user.Role switch
            {
                "user" or "employeeChat" => RedirectToAction("Index","Home"),
                "admin" or "employee" => RedirectToAction("Index", "AdminDashboard"),
                _ => View("Login") // default case 
            };
        }


        [Route("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Route("changePassword")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }
        [Route("changePassword")]
        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> ChangePassword(PassDto data)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("id")?.Value;
                if (userId != null)
                {
                    var check = await _context.Users.FindAsync(int.Parse(userId));
                    if (check == null)
                    {
                        ViewData["Error"] = MessagesConstant.User.AccountNotFound;
                        return View();
                    }
                    else
                    {
                        if (!BCrypt.Net.BCrypt.Verify(data.CurrentPassword, check.Password))
                        {
                            ViewData["Error"] = MessagesConstant.User.WrongPassword;
                            return View();
                        }
                        else
                        {
                            var newPassword = BCrypt.Net.BCrypt.HashPassword(data.NewPassword);
                            check.Password = newPassword;
                            await _context.SaveChangesAsync();
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            TempData["passchange"] = MessagesConstant.User.ChangePasswordSuccess;
                            return RedirectToAction("Login");
                        }
                    }
                }
                return View("ChangePassword");
            }
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
                var data = new Models.User()
                {
                    Username = info.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(info.Pass),
                    Role = info.Role,
                    Email = info.Email,
                };
                await _context.AddAsync(data);
                await _context.SaveChangesAsync();
                TempData["success"] = "Đăng ký tài khoản thành công";
                var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
                return userRoleClaim == "admin" ? RedirectToAction("Index", "AdminDashboard") : RedirectToAction("Login");
            }
            return View("Register");
        }
      
        
        [AllowAnonymous]
        [Route("unauthozied")]
        public IActionResult UnAuthorize()
        {
            return View("unauthorize");
        }

        }
    }