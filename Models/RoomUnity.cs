using Hotel.Atribute;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class RoomUnity : BaseEntity
    {
        [Required(ErrorMessage ="Cần Nhập Tên Tiện Ích")]
        [UniqueName(ErrorMessage = "Tên Tiện Ích đã tồn tại.")]
        public string? Name { get; set; }
        [ForeignKey("Room")]
        public int? RoomId { get; set; }    
    }
}
