
using System.Runtime.InteropServices;

namespace BookOnline.Services.imp
{
    public class BookDetailService : IBookDetailService
    {
        private readonly ApplicationDbContext _context;

        public BookDetailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookDetail> AddAsync(BookDetail bookDetails)
        {
            await _context.AddAsync(bookDetails);
            _context.SaveChanges();
            return bookDetails;
        }

        public BookDetail DeleteBook(BookDetail bookDetails)
        {
            _context.Remove(bookDetails);
            _context.SaveChanges();
            return bookDetails;
        }

        public async Task<IEnumerable<BookDetail>> GetAllAsync(int authorId = 0) // why default value ?
        {
            return await _context.BookDetails
                .Where(b => b.AuthorId == authorId || authorId == 0)
                .Include(b => b.Author)
                .ToListAsync();
            //readable 
        }

        public async Task<BookDetail> GetByIDAsync(int id)
        {
            return await _context.BookDetails
                .SingleOrDefaultAsync(b => b.Id == id);
        }

        public  BookDetail Update(BookDetail bookDetails)
        {
            _context.Update(bookDetails);   
            _context.SaveChanges();
            return bookDetails;
        }
    }
}
