using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCAlbums.Data;
using MVCAlbums.Areas.Identity.Data;

namespace MVCAlbums.Models
{
    public class SeedDatacs
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<MVCAlbumsUser>>();
            IdentityResult roleResult;
            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }
            MVCAlbumsUser user = await UserManager.FindByEmailAsync("admin@mvcmovie.com");
            if (user == null)
            {
                var User = new MVCAlbumsUser();
                User.Email = "admin@mvcmovie.com";
                User.UserName = "admin@mvcmovie.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCAlbumsContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<MVCAlbumsContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Artist.Any() || context.Album.Any() || context.Genre.Any() || context.Review.Any())
                {
                    return; // DB has been seeded
                }

                context.Artist.AddRange(
                new Artist
                {
                    Name = "Taylor Swift",
                    BirthDate = DateTime.Parse("1989-12-13"),
                    Nationality = "American",
                    Gender = "Female",
                    ArtistImg = "https://m.media-amazon.com/images/M/MV5BZGM0YjhkZmEtNGYxYy00OTk0LThlNDgtNGQzM2YwNjU0NDQzXkEyXkFqcGdeQXVyMTU3ODQxNDYz._V1_.jpg"
                },
                new Artist
                {
                    Name = "Sabrina Carpenter",
                    BirthDate = DateTime.Parse("1999-5-11"),
                    Nationality = "American",
                    Gender = "Female",
                    ArtistImg = "https://media.glamourmagazine.co.uk/photos/6411fca91827564a0f927e21/16:9/w_2560%2Cc_limit/SABRINA%2520CARPENTER%252015032023.jpg"
                },
                new Artist
                {
                    Name = "Harry Styles",
                    BirthDate = DateTime.Parse("1994-2-1"),
                    Nationality = "English",
                    Gender = "Male",
                    ArtistImg = "https://people.com/thmb/2Z-Sc2MqOMf9NLl24-A5BRPxo8o=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc():focal(748x255:750x257)/Harry-Styles-111623-ae777cf2d3774350bd74fd8fe501b568.jpg"
                },
                new Artist
                {
                    Name = "Shawn Mendes",
                    BirthDate = DateTime.Parse("1998-8-8"),
                    Nationality = "Canadian",
                    Gender = "Male",
                    ArtistImg = "https://static.wikia.nocookie.net/taylor-swift/images/6/61/Shawn-Mendes.jpg/revision/latest?cb=20230124083859"
                },
                new Artist
                {
                    Name = "Lana Del Rey",
                    BirthDate = DateTime.Parse("1985-6-21"),
                    Nationality = "American",
                    Gender = "Female",
                    ArtistImg = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTrPVs0xRkoxrpch9kCAVV90MLkYUF9idYBGQ&s"
                }
                );
                context.SaveChanges();

                context.Album.AddRange(
                new Album
                {
                    AlbumName = "Midnights",
                    ReleaseDate = DateTime.Parse("2022-11-21"),
                    ArtistId = context.Artist.Single(d => d.Name == "Taylor Swift").Id,
                    AlbumImg = " https://upload.wikimedia.org/wikipedia/en/9/9f/Midnights_-_Taylor_Swift.png",
                    ListenUrl= "https://www.youtube.com/playlist?list=PLxA687tYuMWgXjGLvPvOWqXWgHEIA2JW6"
                },
                new Album
                {
                    AlbumName = "The Tortured Poets Department",
                    ReleaseDate = DateTime.Parse("2024-4-19"),
                    ArtistId = context.Artist.Single(d => d.Name == "Taylor Swift").Id,
                    AlbumImg = "https://www.slugmag.com/wp/wp-content/uploads/2024/05/The-Anthology_Cover.webp",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLDNtAuXIhbEOcnQsArOZd4UW6yvCm5kHF"
                },
                new Album
                {
                    AlbumName = "Folklore",
                    ReleaseDate = DateTime.Parse("2020-7-24"),
                    ArtistId = context.Artist.Single(d => d.Name == "Taylor Swift").Id,
                    AlbumImg = "https://upload.wikimedia.org/wikipedia/en/f/f8/Taylor_Swift_-_Folklore.png",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLxA687tYuMWgvqMTGBnx3RCDP5kbYATH5"
                },
                new Album
                {
                    AlbumName = "Harry's House",
                    ReleaseDate = DateTime.Parse("2022-5-20"),
                    ArtistId = context.Artist.Single(d => d.Name == "Harry Styles").Id,
                    AlbumImg = "https://i.scdn.co/image/ab67616d0000b273b46f74097655d7f353caab14",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLxA687tYuMWgWbfUsntXDsn5HgOz90ka-"
                },
                new Album
                {
                    AlbumName = "Short n' Sweet",
                    ReleaseDate = DateTime.Parse("2024-8-23"),
                    ArtistId = context.Artist.Single(d => d.Name == "Sabrina Carpenter").Id,
                    AlbumImg = "https://upload.wikimedia.org/wikipedia/en/f/fd/Short_n%27_Sweet_-_Sabrina_Carpenter.png",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLxA687tYuMWhAshOEK3Z5kOyZYbN0SCv1"
                },
                new Album
                {
                    AlbumName = "Emails I Can't Send",
                    ReleaseDate = DateTime.Parse("2022-7-15"),
                    ArtistId = context.Artist.Single(d => d.Name == "Sabrina Carpenter").Id,
                    AlbumImg = "https://i.scdn.co/image/ab67616d0000b273700f7bf79c9f063ad0362bdf",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLxA687tYuMWhNDyTKWSPvp0AMZhzFwI9d"
                },
                new Album
                {
                    AlbumName = "Wonder",
                    ReleaseDate = DateTime.Parse("2020-12-4"),
                    ArtistId = context.Artist.Single(d => d.Name == "Shawn Mendes").Id,
                    AlbumImg = "https://i1.sndcdn.com/artworks-YwBuYj6ESUqjCNsE-ahN7Ug-t500x500.jpg",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLxA687tYuMWid5ce_yP7lU4zTOB4hD73h"
                },
                new Album
                {
                    AlbumName = "Born to Die",
                    ReleaseDate = DateTime.Parse("2012-1-27"),
                    ArtistId = context.Artist.Single(d => d.Name == "Lana Del Rey").Id,
                    AlbumImg = "https://upload.wikimedia.org/wikipedia/en/2/29/BornToDieAlbumCover.png",
                    ListenUrl = "https://www.youtube.com/playlist?list=PLo6QvHZ4RyGkpUAZ4StyjDDLaZpct_DIt"
                },
                 new Album
                 {
                     AlbumName = "Lust for Life",
                     ReleaseDate = DateTime.Parse("2017-7-21"),
                     ArtistId = context.Artist.Single(d => d.Name == "Lana Del Rey").Id,
                     AlbumImg = "https://media.pitchfork.com/photos/63ed45f2589919a844be73f0/master/pass/Lana-Del-Rey-Lust-for-Life.jpg",
                     ListenUrl = "https://www.youtube.com/playlist?list=PLlq7Qg7QtlOGYlkgMpHfKJ5jCiWUBsayx"
                 }
                );
                context.SaveChanges();

                context.Genre.AddRange(
                  new Genre { GenreName = "Pop" },/*1*/
                 new Genre { GenreName = "Rock" },/*2*/
                 new Genre { GenreName = "Pop rock" },/*3*/
                 new Genre { GenreName = "Indie pop" },/*4*/
                 new Genre { GenreName = "Indie rock" },/*5*/
                 new Genre { GenreName = "Alternative hip hop" },/*6*/
                 new Genre { GenreName = "Trap" },/*7*/
                 new Genre { GenreName = "Contemporary folk " },/*8*/
                 new Genre { GenreName = "Synth-pop" },/*9*/
                 new Genre { GenreName = "Pop-funk" },/*10*/
                 new Genre { GenreName = "Country" },/*11*/
                 new Genre { GenreName = "Alternative/Indie" },/*12*/
                 new Genre { GenreName = "Indie folk" },/*13*/
                 new Genre { GenreName = "Alternative rock" },/*14*/
                 new Genre { GenreName = "Chamber pop" },/*15*/
                 new Genre { GenreName = "Electropop" }/*16*/
                );
                context.SaveChanges();

                context.AlbumGenre.AddRange(
                new AlbumGenre { AlbumId = 1, GenreId = 9 },
                new AlbumGenre { AlbumId = 1, GenreId = 16 },
                new AlbumGenre { AlbumId = 2, GenreId = 15 },
                new AlbumGenre { AlbumId = 2, GenreId = 9 },
                new AlbumGenre { AlbumId = 3, GenreId = 15 },
                new AlbumGenre { AlbumId = 3, GenreId = 14 },
                new AlbumGenre { AlbumId = 3, GenreId = 13 },
                new AlbumGenre { AlbumId = 4, GenreId = 9 },
                new AlbumGenre { AlbumId = 4, GenreId = 10 },
                new AlbumGenre { AlbumId = 4, GenreId = 3 },
                new AlbumGenre { AlbumId = 5, GenreId = 1 },
                new AlbumGenre { AlbumId = 5, GenreId = 11 },
                new AlbumGenre { AlbumId = 5, GenreId = 12 },
                new AlbumGenre { AlbumId = 6, GenreId = 12 },
                new AlbumGenre { AlbumId = 6, GenreId = 1 },
                new AlbumGenre { AlbumId = 7, GenreId = 3 },
                new AlbumGenre { AlbumId = 8, GenreId = 1 },
                new AlbumGenre { AlbumId = 8, GenreId = 2 },
                new AlbumGenre { AlbumId = 8, GenreId = 3 },
                new AlbumGenre { AlbumId = 8, GenreId = 4 },
                new AlbumGenre { AlbumId = 8, GenreId = 5 },
                new AlbumGenre { AlbumId = 8, GenreId = 6 },
                new AlbumGenre { AlbumId = 8, GenreId = 14 },
                new AlbumGenre { AlbumId = 9, GenreId = 8 },
                new AlbumGenre { AlbumId = 9, GenreId = 7 },
                new AlbumGenre { AlbumId = 9, GenreId = 1 }
                );
                context.SaveChanges();
                
                context.SaveChanges();
                context.Review.AddRange(
                    new Review { AppUser = "Nataly Mack", Comment = "Great!", Rating = 5, AlbumId = 1 },
                    new Review { AppUser = "Ivanna Yang", Comment = "Very Good!", Rating = 4, AlbumId = 1 },
                    new Review { AppUser = "Conrad McClain", Comment = "Good!", Rating = 3, AlbumId = 5 },
                    new Review { AppUser = "Eddie Arias", Comment = "Not Bad!", Rating = 2, AlbumId = 3 },
                    new Review { AppUser = "Aileen Wilson", Comment = "Bad!", Rating = 1, AlbumId = 5 },
                    new Review { AppUser = "Celia Rice", Comment = "Not Bad!", Rating = 2, AlbumId = 4 },
                    new Review { AppUser = "Graham Perkins", Comment = "Great!", Rating = 5, AlbumId = 3 },
                    new Review { AppUser = "Allison Dorsey", Comment = "Great!", Rating = 5, AlbumId = 6 },
                    new Review { AppUser = "Naomi Burns", Comment = "Good!", Rating = 3, AlbumId = 6 },
                    new Review { AppUser = "Cooper French", Comment = "Very Good!", Rating = 4, AlbumId = 2 },
                    new Review { AppUser = "Isabelle Clements", Comment = "Very Good!", Rating = 4, AlbumId = 7 },
                    new Review { AppUser = "Lucy Tran", Comment = "Great!", Rating = 5, AlbumId = 7 },
                    new Review { AppUser = "Millie Good", Comment = "Great!", Rating = 5, AlbumId = 8 },
                    new Review { AppUser = "Jameson Higgins", Comment = "Good!", Rating = 3, AlbumId = 8 },
                    new Review { AppUser = "Camille Hunt", Comment = "Great!", Rating = 5, AlbumId = 9 },
                    new Review { AppUser = "Harvey Ellison", Comment = "Great!", Rating = 5, AlbumId = 9 },
                    new Review { AppUser = "Estella Villegas", Comment = "Great!", Rating = 5, AlbumId = 1 },
                    new Review { AppUser = "Terry Curtis", Comment = "Great!", Rating = 5, AlbumId = 2 },
                    new Review { AppUser = "Robert Petersen", Comment = "Great!", Rating = 5, AlbumId = 3 },
                    new Review { AppUser = "Dawson Mason", Comment = "Great!", Rating = 5, AlbumId = 4 },
                    new Review { AppUser = "Mikaela Howe", Comment = "Great!", Rating = 5, AlbumId = 5 }
                );
                context.SaveChanges();
            }

        }
    }
}
