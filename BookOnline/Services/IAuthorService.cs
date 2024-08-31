using BookOnline.Dto.Update;

namespace BookOnline.Services
{
    public interface IAuthorService
    {
        Task<Response<IEnumerable<Author>>> GetAllAsync();
        Task<Response<Author>> AddAsync(AuthorDto dto);
        Task<Response<Author>> GetByIDAsync(int? id);
        Task<Response<Author>> Update(AuthorDtoUpdate dto);
        Task<Response<Author>> DeleteAuthor(int id);
    }
}
