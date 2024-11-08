using Microsoft.AspNetCore.Identity;
using MVCAlbums.Areas.Identity.Data;

namespace MVCAlbums.Models
{
    public class BoughtAlbums
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public string UserId { get; set; }  // Foreign key for the user
        public MVCAlbumsUser User { get; set; } // Navigation property to the user
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}
