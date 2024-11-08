using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCAlbums.Data;
using MVCAlbums.Models;

namespace MVCAlbums.Controllers
{
    public class BoughtAlbumsController : Controller
    {
        private readonly MVCAlbumsContext _context;

        public BoughtAlbumsController(MVCAlbumsContext context)
        {
            _context = context;
        }

        // GET: BoughtAlbums
        public async Task<IActionResult> Index()
        {
            var mVCAlbumsContext = _context.BoughtAlbums.Include(b => b.Album).Include(b => b.User);
            return View(await mVCAlbumsContext.ToListAsync());
        }

        // GET: BoughtAlbums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boughtAlbums = await _context.BoughtAlbums
                .Include(b => b.Album)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boughtAlbums == null)
            {
                return NotFound();
            }

            return View(boughtAlbums);
        }

        // GET: BoughtAlbums/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: BoughtAlbums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumId,UserId,PurchaseDate")] BoughtAlbums boughtAlbums)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boughtAlbums);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Id", boughtAlbums.AlbumId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", boughtAlbums.UserId);
            return View(boughtAlbums);
        }

        // GET: BoughtAlbums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boughtAlbums = await _context.BoughtAlbums.FindAsync(id);
            if (boughtAlbums == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Id", boughtAlbums.AlbumId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", boughtAlbums.UserId);
            return View(boughtAlbums);
        }

        // POST: BoughtAlbums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumId,UserId,PurchaseDate")] BoughtAlbums boughtAlbums)
        {
            if (id != boughtAlbums.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boughtAlbums);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoughtAlbumsExists(boughtAlbums.Id))
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
            ViewData["AlbumId"] = new SelectList(_context.Album, "Id", "Id", boughtAlbums.AlbumId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", boughtAlbums.UserId);
            return View(boughtAlbums);
        }

        // GET: BoughtAlbums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boughtAlbums = await _context.BoughtAlbums
                .Include(b => b.Album)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (boughtAlbums == null)
            {
                return NotFound();
            }

            return View(boughtAlbums);
        }

        // POST: BoughtAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boughtAlbums = await _context.BoughtAlbums.FindAsync(id);
            if (boughtAlbums != null)
            {
                _context.BoughtAlbums.Remove(boughtAlbums);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoughtAlbumsExists(int id)
        {
            return _context.BoughtAlbums.Any(e => e.Id == id);
        }
    }
}
