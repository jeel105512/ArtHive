using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArtHive.Data;
using ArtHive.Models;

namespace ArtHive.Controllers
{
    public class ArtworksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtworksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Artworks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Artworks.Include(a => a.Collection);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Artworks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .Include(a => a.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        // GET: Artworks/Create
        public IActionResult Create()
        {
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name");
            return View();
        }

        // POST: Artworks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CollectionId,Creator,Description,Price")] Artwork artwork, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                artwork.Image = await UploadPhoto(Image);

                _context.Add(artwork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name", artwork.CollectionId);
            return View(artwork);
        }

        private async Task<string> UploadPhoto(IFormFile Image)
        {
            if (Image == null) return null;
            
            var filePath = Path.GetTempFileName();
            var fileName = Guid.NewGuid() + "-" + Image.FileName;
            var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\images\\artworks\\" + fileName;
            using var stream = new FileStream(uploadPath, FileMode.Create);
            await Image.CopyToAsync(stream);

            return fileName;
        }

        // GET: Artworks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name", artwork.CollectionId);
            return View(artwork);
        }

        // POST: Artworks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CollectionId,Creator,Description,Price")] Artwork artwork, IFormFile? Image)
        {
            if (id != artwork.Id)
            {
                return NotFound();
            }

            // Using the FindAsync method instead of FirstOrDefault to retrieve the existing book.
            var existingArtWork = await _context.Artworks.FindAsync(id);

            if (existingArtWork == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0) // delete the existing file and update it with new one if a file already exists
                {
                    var existingImageLocation = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\images\\artworks\\" + existingArtWork.Image;
                    // delete the existing image
                    FileInfo existingImageFileInfo = new FileInfo(existingImageLocation);
                    if (existingImageFileInfo.Exists)
                    {
                        existingImageFileInfo.Delete();
                    }

                    byte[] fileBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        await Image.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    if (!IsJpegFile(fileBytes) && !IsPngFile(fileBytes))
                    {
                        ModelState.AddModelError("Image", "Only .jpeg or .png files are allowed");
                        ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name", artwork.CollectionId);
                        return View(artwork);
                    }
                    // Save the new image and assign the file name to artwork.Image
                    artwork.Image = await UploadPhoto(Image);
                } else if(existingArtWork.Image != null) // upload the file if the new file is uploaded and there is no existing file
                {
                    // keep the existing photo if no new one uploaded
                    artwork.Image = existingArtWork.Image;
                }
                try
                {
                    // Deatch existingArtWork from the context
                    _context.Entry(existingArtWork).State = EntityState.Detached;
                    
                    existingArtWork.CollectionId = artwork.CollectionId;
                    existingArtWork.Title = artwork.Title;
                    existingArtWork.Creator = artwork.Creator;
                    existingArtWork.Description = artwork.Description;
                    existingArtWork.Price = artwork.Price;

                    // artwork.Image = await UploadPhoto(Image);

                    _context.Update(artwork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtworkExists(artwork.Id))
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
            ViewData["CollectionId"] = new SelectList(_context.Collections, "Id", "Name", artwork.CollectionId);
            return View(artwork);
        }

        // GET: Artworks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artworks == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .Include(a => a.Collection)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (artwork == null)
            {
                return NotFound();
            }

            return View(artwork);
        }

        // POST: Artworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artworks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Artworks'  is null.");
            }
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }

            // Delete the associating image file
            if (!string.IsNullOrEmpty(artwork.Image))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "artworks", artwork.Image);
                if(System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtworkExists(int id)
        {
          return (_context.Artworks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Helper method to check if the file is a JPEG (JFIF) or if the file is a PNG
        private bool IsJpegFile(byte[] fileBytes)
        {
            return fileBytes.Length > 2 && fileBytes[0] == 0xFF && fileBytes[1] == 0xD8;
        }

        private bool IsPngFile(byte[] fileBytes)
        {
            return fileBytes.Length > 8 &&
                fileBytes[0] == 0x89 &&
                fileBytes[1] == 0x50 &&
                fileBytes[2] == 0x4E &&
                fileBytes[3] == 0x47 &&
                fileBytes[4] == 0x0D &&
                fileBytes[5] == 0x0A &&
                fileBytes[6] == 0x1A &&
                fileBytes[7] == 0x0A;
        }
    }
}
