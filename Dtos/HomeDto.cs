using Hotel.Models;

namespace Hotel.Dtos
{
    public class HomeDto
    {
        public List<Banner>? Banners {  get; set; }
        public List<Room>? Rooms { get; set; }
        public List<HotelData>? hotelDatas { get; set; }
        public List<Comment>? comments { get; set; }
       
    }
}
