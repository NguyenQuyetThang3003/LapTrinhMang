using System.ComponentModel.DataAnnotations.Schema;

namespace BlogJavaJS.Models
{
    public class PostImage
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        [Column("url")]
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation property
        public Post? Post { get; set; }
    }
}
