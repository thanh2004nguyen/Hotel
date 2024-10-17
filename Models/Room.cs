using Hotel.Atribute;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Room: BaseEntity
    {

        [ForeignKey("RoomType")]
        public int RoomTypeID { get; set; }
        [Required(ErrorMessage = "Cần Nhập Tên Phòng")]
        public string? RoomName { get; set; }
        [Required(ErrorMessage = "Cần Nhập Số Phòng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số Phòng không được là số âm hoặc 0.")]
        public int RoomNumber { get; set; }
        public virtual RoomType? RoomType { get; set; }
        public int Price { get; set; }
        // Foreign key to RoomPolicy
        public double Rating { get; set; } // Individual rating score
        public int? PolicyId { get; set; } // Add this line
        public virtual RoomPolicy? Policy { get; set; } // Navigation property to RoomPolicy
        public List<RoomWithRoomProperty> RoomWithRoomProperties { get; set; } = new List<RoomWithRoomProperty>();
        public ICollection<Image>? Images { get; set; }
        public virtual ICollection<RoomAmenities> RoomAmenities { get; set; } = new List<RoomAmenities>();
        [Required(ErrorMessage = "Cần Nhập Mô Tả Phòng")]
        public string? Description { get; set; } // Mô tả phòng
        [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100.")]
        public int Discount { get; set; } // Giảm giá (nếu có)
        [Required(ErrorMessage = "Cần Nhập Số Người Lớn Tối Đa")]
        [Range(0, int.MaxValue, ErrorMessage = "Số Người Lớn Tối Đa không được là số âm.")]
        public int MaxAdults { get; set; } // Số người lớn tối đa
        [Required(ErrorMessage = "Cần Nhập Số Trẻ Em Tối Đa")]
        [Range(0, int.MaxValue, ErrorMessage = "Số Trẻ Em Tối Đa không được là số âm.")]
        public int MaxChildren { get; set; } // Số trẻ em tối đa
        public bool Status { get; set; }
        public Room()
        {
            Status = true;
            MaxAdults = 2; 
            MaxChildren = 2; 
            Discount = 0; 
            Rating = 0;
        }
    }
}
