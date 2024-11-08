namespace MVCAlbums.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string GenreName { get; set; }
        public ICollection<AlbumGenre> AlbumGenre { get; set; }
    }
}
