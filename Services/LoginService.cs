using Hotel.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Hotel.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using Hotel.ServiceImql;
using PayPalCheckoutSdk.Orders;
using Newtonsoft.Json.Linq;
using static MessagesConstant;

namespace Hotel.Services
{
    public class LoginService : ILoginService
    {
        private readonly HotelDbContext _context;       //database
        private readonly IConfiguration _configuration;         //cấu hình file config
        public LoginService(IConfiguration configuration, HotelDbContext context) 
        {
            _context = context;
            _configuration = configuration;
        }

        // Tạo token và refresh token
        private string GenerateJwtToken(Models.User user)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //Xác thực token
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.Where(u => u.Email == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return MessagesConstant.User.AccountNotFound;    //khong tim thay tai khoan
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return MessagesConstant.User.WrongPassword;    //sai mat khau
            }

            var token = GenerateJwtToken(user);
            return token;
        }



        //kiểm tra jwt có hợp lệ không
        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                // 1. Lấy khóa secretKey từ cấu hình
                var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

                //tạo tham số xác thực token 
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };

                // 3. Giải mã token
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                // 4. Kiểm tra loại token
                if (validatedToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    // Token hợp lệ
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                // Token không hợp lệ hoặc đã hết hạn
                Console.WriteLine($"Lỗi xác thực token: {ex.Message}");
                return false;
            }
        }



    }
}
