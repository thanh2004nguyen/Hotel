namespace Hotel.Models.Shared
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public BaseEntity() {
        CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        } 
    }
}
