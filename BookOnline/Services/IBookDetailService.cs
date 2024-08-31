using Microsoft.AspNetCore.Mvc;

namespace BookOnline.Services
{
    public interface IBookDetailService
    {
        Task<Response<IEnumerable<BookDetail>>> GetAllAsync();
        Task<Response<BookDetail>> AddAsync(BookDetailsDto bookDetails);
        Task<Response<BookDetail>> GetByIDAsync(int? id);
        Task<Response<BookDetail>> Update(BookDtoUpdate dto);
        Task<Response<BookDetail>> DeleteBook(int id);
                
    }
}
