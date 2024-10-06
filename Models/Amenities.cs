using Hotel.Atribute;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{   //AmenitiesTheme
    public class Amenities : BaseEntity
    {
        [Required(ErrorMessage ="Cần Nhập Tên Tiện Ích")]
        [UniqueName(ErrorMessage = "Tên Tiện Ích đã tồn tại.")]
        public string? Name { get; set; }         
        
        // Khóa ngoại đến IconClass
        public int? IconClassId { get; set; }
        public virtual IconClass IconClass { get; set; }
        public int? AmenitiesThemeId { get; set; }
        public virtual AmenitiesTheme AmenitiesTheme { get; set; }
        public virtual ICollection<RoomAmenities> RoomAmenities { get; set; } = new List<RoomAmenities>();
    }
}
