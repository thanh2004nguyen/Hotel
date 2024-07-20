using Hotel.Dtos;
using Hotel.Models.Shared;

namespace Hotel.Models
{
    public class Banner:BaseEntity
    {
        public string Title { get; set; }   
        public string Content { get; set; } 
        public string Image {  get; set; }

        private Banner() { 
            Title = string.Empty;
            Content = string.Empty;
            Image = string.Empty;
        }

        public Banner(BannerDto data, string image)
        {
            Title = data.Title;
            Content = data.Content;
            Image = image;
        }
    }
}
