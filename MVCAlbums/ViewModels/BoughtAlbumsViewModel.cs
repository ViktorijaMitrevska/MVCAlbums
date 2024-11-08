using MVCAlbums.Models;

namespace MVCAlbums.ViewModels
{
    public class BoughtAlbumsViewModel
    {
        public List<Album> Albums { get; set; }
        public int SelectedAlbums { get; set; }
        public Review Review { get; set; }
    }
}
