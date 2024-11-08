using Microsoft.AspNetCore.Mvc.Rendering;
using MVCAlbums.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCAlbums.ViewModels
{
    public class UploadFileViewModel
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
        [Url]
        [StringLength(int.MaxValue)]
        public string? AlbumImg { get; set; }
        [Url]
        [StringLength(int.MaxValue)]
        public string? ListenUrl { get; set; }
       
        public IFormFile CoverImage { get; set; }
    }
}
