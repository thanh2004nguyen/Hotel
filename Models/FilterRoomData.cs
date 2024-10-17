namespace Hotel.Models
{
    public class FilterRoomData
    {
        public List<int>? RoomTypes { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinRating { get; set; }

    }
}
