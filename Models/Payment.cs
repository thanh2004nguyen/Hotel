using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Payment : BaseEntity
    {
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; } 
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; } // Mã giao dịch thanh toán

        // Foreign key for Booking (1-1 relationship)
        public int BookingId { get; set; }

        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
    }
}
