
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

        public async Task<IEnumerable<BookDetail>> GetAllAsync(int authorId = 0)
        {
            return await _context.BookDetails.Where(b => b.AuthorId == authorId || authorId == 0).Include(b => b.Author).ToListAsync();
        }

        public async Task<Response<BookDetail>> GetByIDAsync(int? id)
        {
            if (id == null)
            {
                return new Response<BookDetail>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                    Value = null
                };
            }
            var res = await _context.BookDetails.Include(a => a.Author).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<BookDetail>
                {
                    ErrorMessage = "Book not found",
                    IsSuccess = false,
                    Value = null
                };
            }
            return new Response<BookDetail>
            {
                ErrorMessage = "",
                IsSuccess = false,
                Value = res
            };
        }

        public  BookDetail Update(BookDetail bookDetails)
        {
            _context.Update(bookDetails);   
            _context.SaveChanges();
            return bookDetails;
        }
    }
}
