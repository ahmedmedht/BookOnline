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
            if (product.IsSuccess == false)
                return BadRequest(product.ErrorMessage);
            else if (product.Value.Count >= dto.count) 
                return BadRequest("There is not enough product");
            

            var cart = new Cart();
            for (int i = 0; i < dto.count; i++)
            {
                cart.ProductId.Add(dto.productId);
                cart.TotalPrice += product.Value.Price;
            }

            product.Value.Count -= dto.count;

            _bookProductService.Update(product.Value);
            await _cartService.AddAsync(cart);

            return Ok(cart);
        }


        [HttpPut("AddNewProductToCart")]
        public async Task<IActionResult> AddNewProductToCart(ProductCartDto dto)
        {
            var cart = await _cartService.GetByIDAsync(dto.Id);
            if (cart.IsSuccess == false)
                return BadRequest(cart.ErrorMessage);

            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product.IsSuccess == false)
                return BadRequest(product.ErrorMessage);
            else if (product.Value.Count >= dto.count)
                return BadRequest("There is not enough product");
            

            
            for (int i = 0; i < dto.count; i++)
            {
                cart.Value.ProductId.Add(dto.productId);
                cart.Value.TotalPrice += product.Value.Price;
            }

            product.Value.Count -= dto.count;
            _bookProductService.Update(product.Value);

            _cartService.Update(cart.Value);

            return Ok(cart);
        }

        [HttpPut("RemoveProductFromCart")]
        public async Task<IActionResult> RemoveProductFromCart(ProductCartDto dto)
        {
            var cart = await _cartService.GetByIDAsync(dto.Id);
            if (cart.IsSuccess == false)
                return BadRequest(cart.ErrorMessage);
            var product = await _bookProductService.GetByIDAsync(dto.productId);
            if (product.IsSuccess == false)
                return BadRequest(product.ErrorMessage);


            for (int i = 0; i < dto.count; i++)
            {
                if(cart.Value.ProductId.Contains(dto.productId))
                    cart.Value.ProductId.Remove(dto.productId);
                else
                {
                    product.Value.Count += i;
                    break;
                }
                cart.Value.TotalPrice -= product.Value.Price;
            }
            
            _bookProductService.Update(product.Value);
            _cartService.Update(cart.Value);

            return Ok(cart);
        }

        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            var cart = await _cartService.GetByIDAsync(cartId);
            if (cart.IsSuccess == false)
                return BadRequest(cart.ErrorMessage);
            if (cart.Value.ProductId.Count > 0)
                return BadRequest("Cart contains products");
            _cartService.DeleteCart(cart.Value);
            return Ok(cart.Value);
        }



    }
}
