using Hotel.Models;

namespace Hotel.ServiceImql
{
    public interface ILoginService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<bool> ValidateTokenAsync(string token);
    }
}
