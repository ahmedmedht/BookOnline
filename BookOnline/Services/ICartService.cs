namespace BookOnline.Services
{
    public interface ICartService
    {
        Task<Response<IEnumerable<Cart>>> GetAllAsync();
        Task<Response<Cart>> AddAsync(ProductCartDto dto);
        Task<Response<Cart>> GetByIDAsync(int? id);
        Task<Response<Cart>> UpdateRemove(CartDtoUpdate dto);
        Task<Response<Cart>> UpdateAdd(CartDtoUpdate cart);
        Task<Response<Cart>> DeleteCart(int id);
    }
}
