using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class RoomPropertyDetail: BaseEntity
    {
        [ForeignKey("RoomProperty")]
        public int RoomPropertyId { get; set; }
     
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        [Required]
        public string? Detail { get; set; }
        [NotMapped]
        public string? Name { get; set; }
        
    }
}
