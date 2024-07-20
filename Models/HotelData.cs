using Hotel.Dtos;
using Hotel.Models.Shared;

namespace Hotel.Models
{
	public class HotelData:BaseEntity
	{
		public string? Type { get; set; }
		public string? Url { get; set; }

		public string? Header {  get; set; }
		public string? Content { get; set; }

        public HotelData() { }
        public HotelData(HomeDataDto data,string img) { 
		Type= data.Type;
		Header= data.Header;
		Content= data.Content;
			Url = img;
		}
    }

	
}
