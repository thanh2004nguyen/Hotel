using Hotel.Dtos;

namespace Hotel.Models
{
    public class IconClass
    {
        public int Id { get; set; }
        public string Icon { get; set; }

        // Mối quan hệ với các lớp khác
        public virtual AmenitiesTheme AmenitiesTheme { get; set; } //1-1
        public virtual ICollection<Amenities> Amenities { get; set; }//2-1
        public virtual ICollection<RoomProperty> RoomProperty { get; set; }
        public virtual ICollection<RoomPolicy> RoomPolicy { get; set; }

        public IconClass(IconCreateDTO dto)
        {
            Icon = dto.Icon;
            // Không cần khởi tạo ID khóa ngoại ở đây, sẽ được khởi tạo trong các thuộc tính riêng lẻ
        }

        public IconClass()
        {
        }
    }
}
