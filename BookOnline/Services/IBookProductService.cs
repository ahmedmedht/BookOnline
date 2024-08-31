namespace BookOnline.Services
{
    public interface IBookProductService
    {
        Task<Response<IEnumerable<BookProduct>>> GetAllAsync();
        Task<Response<BookProduct>> AddAsync(ProductDto dto);
        Task<Response<BookProduct>> GetByIDAsync(int id);
        Task<Response<BookProduct>> Update(ProductDtoUpdate product);
        Task<Response<BookProduct>> DeleteBook(int id);
    }
}
