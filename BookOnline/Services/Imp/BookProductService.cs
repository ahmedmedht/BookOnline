namespace BookOnline.Services.Imp
{
    public class BookProductService : IBookProductService
    {
        private readonly ApplicationDbContext _context;

        public BookProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookProduct> AddAsync(BookProduct product)
        {
            await _context.AddAsync(product);
            _context.SaveChanges();
            return product;
        }

        public BookProduct DeleteBook(BookProduct product)
        {
            _context.Remove(product);
            _context.SaveChanges();
            return product;
        }

        public async Task<IEnumerable<BookProduct>> GetAllAsync()
        {
            return await _context.Products.Include(b => b.BookDetail).ToListAsync();
        }

        public async Task<Response<BookProduct>> GetByIDAsync(int? id)
        {
            if (id == null)
            {
                return new Response<BookProduct>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                    Value = null
                };
            }
            var res = await _context.Products.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<BookProduct>
                {
                    ErrorMessage = "Product not found",
                    IsSuccess = false,
                    Value = null
                };
            }
            return new Response<BookProduct>
            {
                ErrorMessage = "",
                IsSuccess = false,
                Value = res
            };
        }

        public BookProduct Update(BookProduct product)
        {
            _context.Update(product);
            _context.SaveChanges();
            return product;
        }
    }
}

