using BlogJavaJS.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogJavaJS.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }

        // DbSet cho các bảng
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Contact> Contacts { get; set; }   // ✅ thêm bảng liên hệ

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đặt tên bảng
            modelBuilder.Entity<Post>().ToTable("posts");
            modelBuilder.Entity<PostImage>().ToTable("post_images");
            modelBuilder.Entity<Contact>().ToTable("contacts"); // ✅ map bảng thủ công trong MySQL

            // Quan hệ 1-n Post – PostImages
            modelBuilder.Entity<PostImage>()
                .HasOne(pi => pi.Post)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
