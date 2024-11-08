using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAlbums.Models;

namespace MVCAlbums.ViewModels
{
    public class AlbumFilterViewModel
    {
        public IList<Album> Albums{ get; set; }
        public SelectList Genres { get; set; }
        public string AlbumGenre { get; set; }
        public string SearchString { get; set; }
    }
}
