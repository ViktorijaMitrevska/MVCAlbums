using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAlbums.Models;
using System.Collections.Generic;
namespace MVCAlbums.ViewModels
{
    public class AlbumGenreEditViewModel
    {
        public Album Album { get; set; }
        public IEnumerable<int>? SelectedGenres { get; set; }
        public IEnumerable<SelectListItem>? GenreList { get; set; }
    }
}
