using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtHive.Models
{
    public class Artwork
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a Title"), MaxLength(200)]
        public string Title { get; set; }

        public int CollectionId { get; set; }

        [Required(ErrorMessage = "You must provide a Creator Name"), MaxLength(200)]
        [DisplayName("Creator Name")]
        public string Creator { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = "The price must be in range 0.01 - 999999.99"), Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        public Collection? Collection { get; set; } // parent reference
        public List<CartItem>? CartItems { get; set; }
    }
}
