using System.ComponentModel.DataAnnotations;

namespace ArtHive.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        [Required()]
        [Display(Name = "Order")]
        public int OrderId { get; set; }

        [Required()]
        [Display(Name = "Artwork")]
        public int ArtworkId { get; set; }

        [Required(ErrorMessage = "Quantity must be in range 1 - 100"), Range(1, 100)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "The price must be in range 0.01 - 999999.99"), Range(0.01, 999999.99)]
        public double Price { get; set; }
       
        public double Subtotal { get; set; }

        public Order? Order { get; set; } // parent reference
        public Artwork? Artwork { get; set; } // parent reference
    }
}
