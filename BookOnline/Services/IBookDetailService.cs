using Microsoft.AspNetCore.Mvc;

namespace BookOnline.Services
{
    public interface IBookDetailService
    {
        Task<IEnumerable<BookDetail>> GetAllAsync(int AuthorId = 0);

        Task<BookDetail> AddAsync(BookDetail bookDetails);

        Task<BookDetail> GetByIDAsync(int id);

        BookDetail Update(BookDetail bookDetails);

        BookDetail DeleteBook(BookDetail bookDetails);
                
    }
}
