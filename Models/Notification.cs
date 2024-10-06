using System;

namespace Hotel.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public string Content { get; set; }
        public string Image { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public bool IsRead { get; set; } 
        public string Title { get; set; }

        // Constructor
        public Notification(int userId, string content, string image = null, string title = null)
        {
            UserId = userId;
            Content = content;
            Image = image;
            Title = title;
            CreatedAt = DateTime.Now; 
            IsRead = false; 
        }
    }
}
