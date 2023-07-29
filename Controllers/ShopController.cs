using ArtHive.Data;
using ArtHive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            // Get the user's unique identifier (user ID)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Find the active cart for the current user
            var cart = await _context.Carts
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            // If the user doesn't have an active cart, create a new one
            if (cart == null)
            {
                cart = new Cart { UserId = userId };

                // Check if the model state is valid before proceeding
                if (!ModelState.IsValid) return NotFound();

                await _context.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            // Find the artwork with the provided ID
            var artwork = await _context.Artworks
                .FirstOrDefaultAsync(artwork => artwork.Id == artworkId);

            // If the artwork doesn't exist, return a "Not Found" result
            if (artwork == null) return NotFound();

            // Find the cart item for the current artwork and cart
            var cartItem = await _context.CartItems.SingleOrDefaultAsync(cartItem => cartItem.ArtWorkId == artworkId && cartItem.Cart.UserId == userId);
            if (cartItem != null)
            {
                // If the cart item already exists, update its quantity
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                // If the cart item doesn't exist, create a new one
                cartItem = new CartItem
                {
                    Cart = cart,
                    Artwork = artwork,
                    Quantity = quantity,
                    Price = artwork.Price
                };

                // Check if the model state is valid before proceeding
                if (!ModelState.IsValid) return NotFound();

                await _context.AddAsync(cartItem);
            }

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // After successfully adding the item to the cart, redirect to the cart view
            return RedirectToAction("ViewMyCart");
        }

        [Authorize]
        public async Task<IActionResult> ViewMyCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _context.Carts
                .Include(cart => cart.User)
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Artwork)
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            return View(cart);
        }

        [Authorize]
        //[HttpPost]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _context.Carts
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            if(cart == null) return NotFound();

            var cartItem = await _context.CartItems
                .Include(cartItem => cartItem.Artwork)
                .FirstOrDefaultAsync(cartItem => cartItem.Cart == cart && cartItem.Id == cartItemId);

            if (cartItem == null) return NotFound();

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewMyCart");
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _context.Carts
                .Include(cart => cart.User)
                .Include(cart => cart.CartItems)
                .ThenInclude(cartItem => cartItem.Artwork)
                .FirstOrDefaultAsync(cart => cart.UserId == userId && cart.Active == true);

            if(cart == null) return NotFound();

            var order = new Order
            {
                UserId = userId,
                Cart = cart,
                TotalPrice = cart.CartItems.Sum(cartItem => cartItem.Price * cartItem.Quantity),
                Address = null,
                City = null,
                Province = null,
                PostalCode = null,
                Phone = null,
                Email = null,
                PaymentMethod = PaymentMethods.VISA
            };

            ViewData["PaymentMethods"] = new SelectList(Enum.GetValues(typeof(PaymentMethods)));

            return View(order);
        }
    }
}
