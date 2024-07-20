using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Room: BaseEntity
    {

        [ForeignKey("RoomType")]
        public int RoomTypeID { get; set; }
        public bool IsFulled { get; set; }  
        public virtual RoomType? RoomType { get; set; }

        public ICollection<RoomProperty>? roomProperties { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<RoomUnity>? Unities { get; set; }
        public ICollection<RoomPropertyDetail>? Details { get; set; }

        public Room()
        {
            IsFulled=false;
        }


    }
}
