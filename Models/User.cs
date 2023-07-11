using Microsoft.AspNetCore.Identity;

namespace ArtHive.Models
{
    public class User : IdentityUser
    {
        public List<Cart>? Carts { get; set; }

        public List<Order>? Orders { get; set; }
    }
}
