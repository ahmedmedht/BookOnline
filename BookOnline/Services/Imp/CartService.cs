using BookOnline.Model;

namespace BookOnline.Services.Imp
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookProductService _bookProductService;

        public CartService(ApplicationDbContext context, IBookProductService bookProductService)
        {
            _context = context;
            _bookProductService = bookProductService;
        }

        public async Task<Response<Cart>> AddAsync(ProductCartDto dto)
        {
            var cart = new Response<Cart>();
            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product.IsSuccess == false)
            {
                cart.ErrorMessage = product.ErrorMessage;
                cart.IsSuccess = false;
                return cart;
            }
            else if (product.Value.Count >= dto.count)
            {
                cart.ErrorMessage = "There is not enough product";
                cart.IsSuccess=false;
                return cart;
            }

            for (int i = 0; i < dto.count; i++)
            {
                cart.Value.ProductId.Add(dto.productId);
                cart.Value.TotalPrice += product.Value.Price;
            }

            product.Value.Count -= dto.count;
            try
            {
                _context.Update(product.Value);

                await _context.AddAsync(cart.Value);
                _context.SaveChanges();

                cart.IsSuccess = true;
                return cart;
            }
            catch (Exception ex) { 
                cart.ErrorMessage = ex.Message;
                cart.IsSuccess = false;
                return cart;
            }
        }

        public async Task<Response<Cart>> DeleteCart(int id)
        {
            var cart = await GetByIDAsync(id);
            if (cart.IsSuccess == false)
                return cart;

            if (cart.Value.ProductId.Count > 0)
            {
                cart.ErrorMessage = "Cart contains products";
                cart.IsSuccess=false;
                return cart;
            }
            try
            {
                _context.Remove(cart.Value);
                _context.SaveChanges();
                return cart;
            }
            catch (Exception ex) {
                cart.ErrorMessage = ex.Message;
                cart.IsSuccess = false;
                return cart;
            }
        }

        public async Task<Response<IEnumerable<Cart>>> GetAllAsync()
        {
            try
            {
                var carts = await _context.Carts.Include(b => b.Product).ToListAsync();
                return new Response<IEnumerable<Cart>>
                {
                    IsSuccess = true,
                    Value = carts
                };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<Cart>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Response<Cart>> GetByIDAsync(int? id)
        {
            if (id == null)
            {
                return new Response<Cart>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                };
            }
            var res = await _context.Carts.Include(a => a.Product).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<Cart>
                {
                    ErrorMessage = "Cart not found",
                    IsSuccess = false,
                };
            }
            return new Response<Cart>
            {
                ErrorMessage = "",
                IsSuccess = true,
                Value = res
            };
        }

        public async Task<Response<Cart>> UpdateAdd(CartDtoUpdate dto)
        {
            var cart = await GetByIDAsync(dto.Id);
            if (!cart.IsSuccess)
                return cart;

            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product.IsSuccess == false)
            {
                cart.ErrorMessage = product.ErrorMessage;
                cart.IsSuccess = false;
                return cart;
            }
            else if (product.Value.Count >= dto.count)
            {
                cart.ErrorMessage = "There is not enough product";
                cart.IsSuccess = false;
                return cart;
            }

            for (int i = 0; i < dto.count; i++)
            {
                cart.Value.ProductId.Add(dto.productId);
                cart.Value.TotalPrice += product.Value.Price;
            }

            product.Value.Count -= dto.count;
            try
            {
                _context.Update(product.Value);

                _context.Update(cart.Value);
                _context.SaveChanges();

                cart.IsSuccess = true;
                return cart;
            }
            catch (Exception ex)
            {
                cart.ErrorMessage = ex.Message;
                cart.IsSuccess = false;
                return cart;
            }
        }

        public async Task<Response<Cart>> UpdateRemove(CartDtoUpdate dto)
        {
            var cart = await GetByIDAsync(dto.Id);
            if (!cart.IsSuccess)
                return cart;

            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product.IsSuccess == false)
            {
                cart.ErrorMessage = product.ErrorMessage;
                cart.IsSuccess = false;
                return cart;
            }

            for (int i = 0; i <= dto.count; i++)
            {
                if (cart.Value.ProductId.Contains(dto.productId))
                {
                    cart.Value.ProductId.Remove(dto.productId);
                    cart.Value.TotalPrice -= product.Value.Price;
                }
                else
                {
                    product.Value.Count += i;
                    break;
                }
            }

            try
            {
                _context.Update(product.Value);

                _context.Update(cart.Value);
                _context.SaveChanges();

                cart.IsSuccess = true;
                return cart;
            }
            catch (Exception ex)
            {
                cart.ErrorMessage = ex.Message;
                cart.IsSuccess = false;
                return cart;
            }
        }
    }
}

