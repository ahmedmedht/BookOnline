namespace BookOnline.Services.imp

{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> AddAsync(Author author)
        {
            await _context.AddAsync(author);
            _context.SaveChanges();
            return  author;
        }

        public Author DeleteAuthor(Author author)
        {
            _context.Remove(author);
            _context.SaveChanges();
            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Author.Include(b => b.BookDetail).ToListAsync();
        }

        public async Task<Response<Author>> GetByIDAsync(int? id)
        {
            if (id == null)
            {
                return new Response<Author>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                    Value = null
                };
            }
            var res = await _context.Author.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<Author>
                {
                    ErrorMessage = "Author not found",
                    IsSuccess = false,
                    Value = null
                };
            }
            return new Response<Author>
            {
                ErrorMessage = "",
                IsSuccess = false,
                Value = res
            };
        }

        public Author Update(Author author)
        {
            _context.Update(author);
            _context.SaveChanges();
            return author;
        }
        
    }
}
