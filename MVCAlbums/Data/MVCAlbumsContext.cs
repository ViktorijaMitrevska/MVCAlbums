using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCAlbums.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVCAlbums.Areas.Identity.Data;
namespace MVCAlbums.Data
{
    public class MVCAlbumsContext : IdentityDbContext<MVCAlbumsUser>
    {
        public MVCAlbumsContext (DbContextOptions<MVCAlbumsContext> options)
            : base(options)
        {
        }

        public DbSet<MVCAlbums.Models.Album> Album { get; set; } = default!;
        public DbSet<MVCAlbums.Models.Artist> Artist { get; set; } = default!;
        public DbSet<MVCAlbums.Models.Review> Review { get; set; } = default!;
        public DbSet<MVCAlbums.Models.Genre> Genre { get; set; } = default!;
        public DbSet<MVCAlbums.Models.AlbumGenre> AlbumGenre { get; set; } = default!;
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


        }
        public DbSet<MVCAlbums.Models.BoughtAlbums> BoughtAlbums { get; set; } = default!;
       
        
    }
}
