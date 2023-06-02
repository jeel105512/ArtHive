using System.ComponentModel.DataAnnotations;

namespace ArtHive.Models
{
    public class Collection
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must name a Collection")]
        [Display(Name = "Collection Name")]
        public string Name { get; set;}

        public List<Artwork>? Artworks { get; set; } // child reference
    }
}
