using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class RoomPolicy : BaseEntity
    {
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Content { get; set; }

        // Khóa ngoại đến IconClass
        public int? IconClassId { get; set; }
        public virtual IconClass? IconClass { get; set; }
    }
}
