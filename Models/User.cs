using Hotel.Models.Shared;

namespace Hotel.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public bool IsOnline { get; set; } = false;

    }
}
