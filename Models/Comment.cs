using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Comment: BaseEntity
    {
        public string? Name {  get; set; }
        public string? Content { get; set; } 
        public string? avatar { get; set; }
        public  int start {  get; set; }
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        [NotMapped]
        public string? RoomType {  get; set; }
    }
}
