using Azure;

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

        public async Task<Response<Author>> GetByIDAsync(int id)
        {
            var res =await _context.Author.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id);
            if(res == null)
            {
                return new Response<Author>
                {
                    ErrorMessage = "not found",
                    Success = false,
                    Result = null

                };
            }

            return new Response<Author>
            {
                ErrorMessage = "",
                Success = true,
                Result = res

            };
            //return await _context.Author.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id); // check null condition ? never trust client side 
        }
        public class Response<T> where T : class // Genaric Topic
        {
            public T Result { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        public Author Update(Author author)
        {
            _context.Update(author);
            _context.SaveChanges();
            return author;
        }
    }
}
