namespace BookOnline.Services
{
    public interface IBookProductService
    {
        Task<IEnumerable<BookProduct>> GetAllAsync();
        Task<BookProduct> AddAsync(BookProduct bookProduct);
        Task<Response<BookProduct>> GetByIDAsync(int? id);
        BookProduct Update(BookProduct product);
        BookProduct DeleteBook(BookProduct product);
    }
}
