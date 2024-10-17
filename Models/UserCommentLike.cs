using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class UserCommentLike : BaseEntity
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }

        public Comment? Comment { get; set; }
        public User? User { get; set; }
    }
}