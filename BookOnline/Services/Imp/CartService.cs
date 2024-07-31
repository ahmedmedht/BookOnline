namespace BookOnline.Services.Imp
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddAsync(Cart cart)
        {
            await _context.AddAsync(cart);
            _context.SaveChanges();
            return cart;
        }

        public Cart DeleteCart(Cart cart)
        {
            _context.Remove(cart);
            _context.SaveChanges();
            return cart;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _context.Carts.Include(b => b.Product).ToListAsync();
        }

        public async Task<Cart> GetByIDAsync(int id)
        {
            return await _context.Carts.Include(b => b.Product).SingleOrDefaultAsync(b => b.Id == id);
        }

        public Cart Update(Cart cart)
        {
            _context.Update(cart);
            _context.SaveChanges();
            return cart;
        }
    }
}

