using System;
using Microsoft.EntityFrameworkCore;

namespace netshop.Models
{
    public class NetshopDbContext : DbContext
    {
		public NetshopDbContext(DbContextOptions<NetshopDbContext> options)
            : base(options)
        {
		}

		public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
