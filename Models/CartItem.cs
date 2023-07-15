using System.ComponentModel.DataAnnotations;

namespace ArtHive.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        
        public int ArtWorkId { get; set; }
        
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity must be in range 1 - 100"), Range(1, 100)]
        public int Quantity { get; set; }

        public Artwork? Artwork { get; set; } // parent reference

        public Cart? Cart { get; set; }
    }
}
