using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogJavaJS.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Content { get; set; }

        [MaxLength(50)]
        public string? Tag { get; set; }

        // Ảnh đại diện
        public string? ImageUrl { get; set; }

        // Chỉ dùng để nhận file upload, không map DB
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int Views { get; set; }

        // Bài nổi bật
        public bool IsFeatured { get; set; } = false;

        // Bộ sưu tập ảnh
        public ICollection<PostImage> Images { get; set; } = new List<PostImage>();
    }
}
