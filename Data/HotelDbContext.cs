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
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RoomProperty> RoomProperties { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Amenities> Amenities { get; set; }
        public DbSet<AmenitiesTheme> AmenitiesThemes { get; set; }
        public DbSet<RoomPolicy> RoomPolicies { get; set; }
        public DbSet<HotelData> hotelDatas { get; set; }
        public DbSet<Comment>Comments  { get; set; }
        public DbSet<UserCommentLike> UserCommentLikes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<IconClass> IconClasses { get; set; }
        public DbSet<RoomAmenities> RoomAmenities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomAmenities>()
                .HasKey(ra => new { ra.RoomId, ra.AmenitiesId });

            modelBuilder.Entity<RoomAmenities>()
                .HasOne(ra => ra.Room)
                .WithMany(r => r.RoomAmenities)
                .HasForeignKey(ra => ra.RoomId);

            modelBuilder.Entity<RoomAmenities>()
                .HasOne(ra => ra.Amenities)
                .WithMany(a => a.RoomAmenities)
                .HasForeignKey(ra => ra.AmenitiesId);
        }
    }
}
