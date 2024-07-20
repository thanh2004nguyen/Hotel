using Hotel.Atribute;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class RoomProperty : BaseEntity
    {
        [Required(ErrorMessage = "Cần Nhập Tên Property")]
        [UniqueName(ErrorMessage = "Tên Property đã tồn tại.")]
        public string? Name { get; set; }
        public ICollection<RoomPropertyDetail>? Details { get; set; }
      
    }
}
