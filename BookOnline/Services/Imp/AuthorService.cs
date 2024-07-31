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

        public async Task<Author> GetByIDAsync(int id)
        {
            return await _context.Author.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id);
        }

        public Author Update(Author author)
        {
            _context.Update(author);
            _context.SaveChanges();
            return author;
        }
    }
}
