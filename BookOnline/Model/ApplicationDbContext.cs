using System.Reflection.Emit;

namespace BookOnline.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<BookDetail> BookDetails { get; set; }
        public DbSet<BookProduct> Products { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ImageInfo> Images { get; set; }
    }
}
