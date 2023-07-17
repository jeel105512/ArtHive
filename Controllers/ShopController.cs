using ArtHive.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtHive.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> ShopAll()
        {
            var artworks = await _context.Artworks
                .OrderBy(artwork => artwork.Title)
                .ToListAsync();

            return View(artworks);
        }

        public async Task<IActionResult> ShopByCollection()
        {
            var collections = await _context.Collections
                .OrderBy(collection => collection.Name)
                .Include(collection => collection.Artworks) // to get access of the artworks in the ShopByCollection() Action
                .ToListAsync();

            return View(collections);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var collectionWithArtworks = await _context.Collections
                .Include(collection => collection.Artworks)
                .FirstOrDefaultAsync(collection => collection.Id == id);

            return View(collectionWithArtworks);
        }

        public async Task<IActionResult> ArtworkDetails(int? id)
        {
            if (id == null) return NotFound();

            var artwork = await _context.Artworks.FindAsync(id);
            if(artwork == null) return NotFound();

            return View(artwork);
        }
    }
}
