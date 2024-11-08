using System.ComponentModel.DataAnnotations;

namespace MVCAlbums.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }

        [Url]
        [StringLength(int.MaxValue)]
        public string? AlbumImg { get; set; }
        [Url]
        [StringLength(int.MaxValue)]
        public string? ListenUrl { get; set; }
        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
        public ICollection<AlbumGenre>? AlbumGenres { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public double AverageRating()
        {
            if (Reviews == null || Reviews.Count == 0)
            {
                return 0;
            }
            int sumOfRatings = 0;
            foreach (var item in Reviews)
            {
                if (item.Rating != null)
                {
                    sumOfRatings += (int)item.Rating;

                }
            }
            double averageRating = (double)sumOfRatings / Reviews.Count;
            return averageRating;
        }

    }
}
