using System.ComponentModel.DataAnnotations;

namespace MVCAlbums.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? About { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string? Nationality { get; set; }

        [StringLength(50)]
        public string? Gender { get; set; }

        [StringLength(int.MaxValue)]
        public string? ArtistImg { get; set; }
        public ICollection<Album>? Albums { get; set; }
    }
}
