using Hotel.Atribute;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
    public class RoomType : BaseEntity
    {

        [Required(ErrorMessage = "Cần Nhập Type")]
    
        public string? Type { get; set; }
		[Required(ErrorMessage = "Cần Nhập Type")]
		public string? Description { get; set; }

       

    }
}
