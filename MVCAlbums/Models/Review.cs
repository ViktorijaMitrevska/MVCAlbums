using System.ComponentModel.DataAnnotations;

namespace MVCAlbums.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        [StringLength(450)]
        public string AppUser { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public int? Rating { get; set; }
    }
}
