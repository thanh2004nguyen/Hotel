namespace Hotel.Dtos
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string SmokingPreference { get; set; }
        public string BedPreference { get; set; }
        public string SpecialRequest { get; set; } // Yêu cầu đặc biệt
        public decimal Price { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int TotalNights { get; set; }

        // Thông tin thanh toán
        public string PaymentMethod { get; set; } // Phương thức thanh toán
        public decimal PaymentAmount { get; set; } // Số tiền thanh toán
        public DateTime PaymentDate { get; set; } // Ngày thanh toán
        public string TransactionId { get; set; } // Mã giao dịch thanh toán
    }
}
