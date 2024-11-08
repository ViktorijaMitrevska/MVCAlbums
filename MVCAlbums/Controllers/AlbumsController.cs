using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCAlbums.Data;
using MVCAlbums.Models;
using MVCAlbums.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MVCAlbums.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;

namespace MVCAlbums.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MVCAlbumsContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<MVCAlbumsUser> _userManager;

        public AlbumsController(MVCAlbumsContext context, IWebHostEnvironment webHostEnvironment, UserManager<MVCAlbumsUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Albums
        public async Task<IActionResult> Index(string searchstring)
        {
            IQueryable<Album> albums = _context.Album.Include(m => m.Artist).Include(m => m.AlbumGenres).ThenInclude(m => m.Genre).Include(m => m.Reviews);
            IQueryable<string> title = _context.Album.OrderBy(b => b.AlbumName).Select(b => b.AlbumName).Distinct();

            if (!string.IsNullOrEmpty(searchstring))
            {
                albums = albums.Where(b =>
                b.AlbumName.Contains(searchstring) ||
                b.Artist.Name.Contains(searchstring) ||
                b.AlbumGenres.Any(b => b.Genre.GenreName.Contains(searchstring)));
            }

            foreach (var album in albums)
            {
                var distinctGenres = album.AlbumGenres
                    .Where(b => b.Genre != null)
                    .GroupBy(b => b.GenreId)
                    .Select(b => b.First())
                    .ToList();

                album.AlbumGenres = distinctGenres;
            }
            var filterVM = new AlbumFilterViewModel
            {
                Albums = await albums.ToListAsync(),
                Genres = new SelectList(await title.ToListAsync())
            };

            return View(filterVM);
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Artist)
                .Include(m => m.AlbumGenres).ThenInclude(m => m.Genre)
                .Include(b=>b.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Albums/Create
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "Id", "Name");
            ViewData["Genres"] = new SelectList(_context.Genre, "Id", "GenreName");
            return View();
        }

        // POST: Albums/Create
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumName,ReleaseDate,ArtistId,ListenUrl")] Album album, IFormFile albumImgFile, List<int> SelectedGenres)
        {
            if (ModelState.IsValid)
            {
                if (albumImgFile != null && albumImgFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(albumImgFile.FileName);
                    var extension = Path.GetExtension(albumImgFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await albumImgFile.CopyToAsync(stream);
                    }

                    album.AlbumImg = "/images/" + uniqueFileName;
                }

                _context.Add(album);
                await _context.SaveChangesAsync();
                foreach (var genreId in SelectedGenres)
                {
                    var albumGenre = new AlbumGenre
                    {
                        AlbumId = album.Id,
                        GenreId = genreId
                    };
                    _context.AlbumGenre.Add(albumGenre);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ArtistId"] = new SelectList(_context.Artist, "Id", "Name", album.ArtistId);
            ViewData["Genres"] = new SelectList(_context.Genre, "Id", "GenreName");

            return View(album);
        }

        // GET: Albums/Edit/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = _context.Album.Where(m => m.Id == id).Include(m => m.AlbumGenres).FirstOrDefault();
            if (album == null)
            {
                return NotFound();
            }

            var genres = _context.Genre.AsEnumerable();
            AlbumGenreEditViewModel viewmodel = new AlbumGenreEditViewModel
            {
                Album = album,
                GenreList = new MultiSelectList(genres, "Id", "GenreName"),
                SelectedGenres = album.AlbumGenres.Select(sa => sa.GenreId)
            };

            ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "Id", "Name", album.ArtistId);
            return View(viewmodel);
        }

        // POST: Albums/Edit/5
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlbumGenreEditViewModel viewmodel)
        {
            if (id != viewmodel.Album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Album);
                    await _context.SaveChangesAsync();

                    var newGenreList = viewmodel.SelectedGenres;
                    var prevGenreList = _context.AlbumGenre.Where(s => s.AlbumId == id).Select(s => s.GenreId);
                    var toBeRemoved = _context.AlbumGenre.Where(s => s.AlbumId == id);

                    if (newGenreList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newGenreList.Contains(s.GenreId));

                        foreach (var genreId in newGenreList)
                        {
                            if (!prevGenreList.Any(s => s == genreId))
                            {
                                _context.AlbumGenre.Add(new AlbumGenre { AlbumId = id, GenreId = genreId });
                            }
                        }
                    }

                    _context.AlbumGenre.RemoveRange(toBeRemoved);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(viewmodel.Album.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ArtistId"] = new SelectList(_context.Set<Artist>(), "Id", "Name", viewmodel.Album.ArtistId);
            return View(viewmodel);
        }

        // GET: Albums/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Album
                .Include(a => a.Artist)
                .Include(m => m.AlbumGenres).ThenInclude(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Album.FindAsync(id);
            if (album != null)
            {
                _context.Album.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Album.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int id)
        {
            var album = await _context.Album.FindAsync(id);
            if (album == null)
            {
                return NotFound(); // Album not found
            }

            var userId = _userManager.GetUserId(User); // Get the current user's ID
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // User not logged in
            }

            // Check if the album is already bought by the user
            var existingPurchase = await _context.BoughtAlbums
                .FirstOrDefaultAsync(b => b.AlbumId == id && b.UserId == userId);

            if (existingPurchase == null)
            {
                // Add the album to the BoughtAlbums table
                var boughtAlbum = new BoughtAlbums
                {
                    AlbumId = album.Id,
                    UserId = userId,
                    PurchaseDate = DateTime.Now
                };

                _context.BoughtAlbums.Add(boughtAlbum);
                await _context.SaveChangesAsync(); // Save changes to the database
            }

            // Redirect to the MyBoughtAlbums action to show the user's purchased albums
            return RedirectToAction(nameof(MyBoughtAlbums));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Buy(int id)
        //{
        //    var album = await _context.Album.FindAsync(id);
        //    if (album == null)
        //    {
        //        return NotFound();
        //    }

        //    // Get the current user
        //    var userId = _userManager.GetUserId(User);
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(); // User is not logged in
        //    }

        //    // Check if the album is already bought by this user
        //    var existingPurchase = await _context.BoughtAlbums
        //        .FirstOrDefaultAsync(b => b.AlbumId == id && b.UserId == userId);

        //    if (existingPurchase == null)
        //    {
        //        // Create a new BoughtAlbum entry
        //        var boughtAlbum = new BoughtAlbums
        //        {
        //            AlbumId = album.Id,
        //            Album = album,
        //            UserId = userId,
        //            PurchaseDate = DateTime.Now
        //        };

        //        _context.BoughtAlbums.Add(boughtAlbum);
        //        await _context.SaveChangesAsync();
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
        public async Task<IActionResult> MyBoughtAlbums()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var boughtAlbums = await _context.BoughtAlbums
                .Where(b => b.UserId == userId)
                .Include(b => b.Album)
                .ToListAsync();

            return View(boughtAlbums);
        }
        //public async Task<IActionResult> BuyAlbum(int id)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return Challenge();
        //    }

        //    var album = await _context.Album.FindAsync(id);
        //    if (album == null)
        //    {
        //        return NotFound();
        //    }

        //    var userAlbum = new UserBook
        //    {
        //        AppUser = user.Id,
        //        AlbumId = album.Id,
        //    };

        //    _context.UserBook.Add(userAlbum);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction(nameof(MyBooks));
        //}
        //public async Task<IActionResult> MyBooks()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return Challenge();
        //    }

        //    var userAlbums = await _context.UserBook
        //        .Include(ub => ub.Book)
        //        .Where(ub => ub.AppUser == user.Id)
        //        .ToListAsync();

        //    return View(userAlbums);
        //}

    }
}
