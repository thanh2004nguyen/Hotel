﻿using Hotel.Atribute;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Models
{
 
    public class RoomProperty : BaseEntity
    {
        [Required(ErrorMessage = "Cần Nhập Tên Property")]
        [UniqueName(ErrorMessage = "Tên Property đã tồn tại.")]
        public string? Name { get; set; }

        // Khóa ngoại đến IconClass
        public int? IconClassId { get; set; }
        public virtual IconClass? IconClass { get; set; }
        // Thiết lập quan hệ nhiều-nhiều với Room thông qua bảng trung gian
        public ICollection<RoomWithRoomProperty>? RoomRoomProperties { get; set; }
    }
}
//AmenitiesTheme