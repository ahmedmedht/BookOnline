namespace BookOnline.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> AddAsync(Cart cart);
        Task<Cart> GetByIDAsync(int id);
        Cart Update(Cart cart);
        Cart DeleteCart(Cart cart);
    }
}
