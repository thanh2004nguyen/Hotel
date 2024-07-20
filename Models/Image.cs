using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Image : BaseEntity
    {
 
        public string? Url { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFile { get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        [NotMapped]
        public string? RoomType { get; set; }
    }
}
