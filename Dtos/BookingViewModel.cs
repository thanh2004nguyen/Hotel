using Hotel.Models;

namespace Hotel.Dtos
{
    public class BookingViewModel
    {
            public Booking Booking { get; set; }
            public List<Payment> Payments { get; set; }
        
    }
}
