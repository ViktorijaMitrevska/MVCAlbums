using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAlbums.Models;

namespace MVCAlbums.ViewModels
{
    public class ArtistFilterViewModel
    {
        public Artist Artist { get; set; }
        public IEnumerable<int?> SelectedAlbums { get; set; }
        public IEnumerable<SelectListItem>? AlbumList { get; set; }
        public IEnumerable<Album> AlbumByArtist { get; set; }
    }
}
