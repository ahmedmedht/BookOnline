using BookOnline.Dto;
using BookOnline.Services;
using BookOnline.Services.imp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IBookProductService _bookProductService;
        private readonly ICartService _cartService;

        public CartController(IBookProductService bookProductService, ICartService cartService)
        {
            _bookProductService = bookProductService;
            _cartService = cartService;
        }

        [HttpGet("GetAllCart")]
        public async Task<IActionResult> GetAllCart()
        {
            var carts = await _cartService.GetAllAsync();
            return Ok(carts);

        }
        [HttpPost("AddProductToNewCart")]
        public async Task<IActionResult> AddNewCart(ProductCartDto dto)
        {
            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product == null)
            {
                return BadRequest("Product isn't found");
            }
            else if (product.Count >= dto.count) {
                return BadRequest("There is not enough product");
            }

            var cart = new Cart();
            for (int i = 0; i < dto.count; i++)
            {
                cart.ProductId.Add(dto.productId);
                cart.TotalPrice += product.price;
            }
            product.Count -= dto.count;
            _bookProductService.Update(product);
            await _cartService.AddAsync(cart);


            

            return Ok(cart);
        }
        [HttpPut("AddNewProductToCart")]
        public async Task<IActionResult> AddNewProductToCart(int cartId,ProductCartDto dto)
        {
            var cart = await _cartService.GetByIDAsync(cartId);
            if (cart == null)
            {
                return BadRequest("Cart isn't found");

            }

            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product == null)
            {
                return BadRequest("Product isn't found");
            }
            else if (product.Count >= dto.count)
            {
                return BadRequest("There is not enough product");
            }

            
            for (int i = 0; i < dto.count; i++)
            {
                cart.ProductId.Add(dto.productId);
                cart.TotalPrice += product.price;
            }
            product.Count -= dto.count;
            _bookProductService.Update(product);

            _cartService.Update(cart);

            return Ok(cart);
        }

        [HttpPut("RemoveProductFromCart")]
        public async Task<IActionResult> RemoveProductFromCart(int cartId, ProductCartDto dto)
        {
            var cart = await _cartService.GetByIDAsync(cartId);
            if (cart == null)
            {
                return BadRequest("Cart isn't found");
            }
            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product == null)
            {
                return BadRequest("Product isn't found");
            }
            
            
            for (int i = 0; i < dto.count; i++)
            {
                if(cart.ProductId.Contains(dto.productId))
                    cart.ProductId.Remove(dto.productId);
                else
                {
                    product.Count += i;
                    break;
                }
                cart.TotalPrice -= product.price;
            }
            
            _bookProductService.Update(product);

            _cartService.Update(cart);

            return Ok(cart);
        }

        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            var cart = await _cartService.GetByIDAsync(cartId);
            if (cart == null)
            {
                return BadRequest("Cart isn't found");
            }
            if(cart.ProductId.Count > 0)
                return BadRequest("Cart contains products");
            _cartService.DeleteCart(cart);
            return Ok(cart);
        }



    }
}
