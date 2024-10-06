using Hotel.Models.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    // Lớp lưu trữ thông tin chủ đề tiện nghi
    public class AmenitiesTheme : BaseEntity
    {
        [Required(ErrorMessage = "Cần Nhập Tên Chủ Đề")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Cần Chọn Icon")]
        public int? IconClassId { get; set; }
        public virtual IconClass IconClass { get; set; }
        // Danh sách tiện nghi
        public virtual ICollection<Amenities> Amenities { get; set; } = new List<Amenities>();
    }
}
