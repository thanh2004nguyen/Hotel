using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options): base(options) 
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Banner> Banners { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<RoomProperty> RoomProperties { get; set; }
        public DbSet<RoomPropertyDetail> RoomPropertyDetails { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomUnity> RoomUnities { get; set; }
        public DbSet<RoomPolicy> RoomPolicies { get; set; }
        public DbSet<HotelData> hotelDatas { get; set; }
        public DbSet<Comment>Comments  { get; set; }

    }
}
