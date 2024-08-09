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

        public async Task<Response<Cart>> GetByIDAsync(int? id)
        {
            if (id == null)
            {
                return new Response<Cart>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                    Value = null
                };
            }
            var res = await _context.Carts.Include(a => a.Product).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<Cart>
                {
                    ErrorMessage = "Cart not found",
                    IsSuccess = false,
                    Value = null
                };
            }
            return new Response<Cart>
            {
                ErrorMessage = "",
                IsSuccess = true,
                Value = res
            };
        }

        public Cart Update(Cart cart)
        {
            _context.Update(cart);
            _context.SaveChanges();
            return cart;
        }
    }
}

