namespace ArtHive.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public bool Active { get; set; } = true;

        public User? User { get; set; }

        public List<CartItem>? CartItems { get; set; }
    }
}
