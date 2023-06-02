using ArtHive.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtHive.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}