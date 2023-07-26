using ArtHive.Data;
using ArtHive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        
        public async Task<IActionResult> ShopByCollectionDetails(int? id)
        {
            var collectionWithArtworks = await _context.Collections
                .Include(collection => collection.Artworks)
                .FirstOrDefaultAsync(collection => collection.Id == id);

            return View(collectionWithArtworks);
        }

        public async Task<IActionResult> ArtworkDetails(int? id, string backAction)
        {
            if (id == null) return NotFound();

            var artwork = await _context.Artworks.FirstOrDefaultAsync(artwork => artwork.Id == id);
            if(artwork == null) return NotFound();

            ViewBag.BackAction = backAction;

            return View(artwork);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int artworkId, int quantity)
        {
            //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User == null) return NotFound();

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.Carts
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };

                if (!ModelState.IsValid) return NotFound();
                
                await _context.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            var artwork = await _context.Artworks
                .FirstOrDefaultAsync(artwork => artwork.Id == artworkId);

            if (artwork == null) return NotFound();

            var cartItem = new CartItem
            {
                Cart = cart,
                Artwork = artwork,
                Quantity = quantity,
                Price = (decimal)artwork.Price
            };

            if (!ModelState.IsValid) return NotFound();

            await _context.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("ArtworkDetails", "Shop", new { id = artworkId });
        }
    }
}
