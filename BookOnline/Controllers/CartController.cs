using BookOnline.Dto;
using BookOnline.Model;
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
        private readonly ICartService _cartService;

        public CartController( ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetAllCart")]
        public async Task<IActionResult> GetAllCart()
        {
            var carts = await _cartService.GetAllAsync();
            if (!carts.IsSuccess)
                return BadRequest(carts.ErrorMessage);

            return Ok(carts.Value);

        }

        [HttpPost("AddProductToNewCart")]
        public async Task<IActionResult> AddNewCart(ProductCartDto dto)
        {
            var cart = await _cartService.AddAsync(dto);

            if (!cart.IsSuccess)
                return BadRequest(cart.ErrorMessage);

            return Ok(cart.Value);
        }


        [HttpPut("AddNewProductToCart")]
        public async Task<IActionResult> AddNewProductToCart(CartDtoUpdate dto)
        {
            var cart = await _cartService.UpdateAdd(dto);

            if (!cart.IsSuccess)
                return BadRequest(cart.ErrorMessage);

            return Ok(cart.Value);
        }

        [HttpPut("RemoveProductFromCart")]
        public async Task<IActionResult> RemoveProductFromCart(CartDtoUpdate dto)
        {
            var cart = await _cartService.UpdateRemove(dto);

            if (!cart.IsSuccess)
                return BadRequest(cart.ErrorMessage);

            return Ok(cart.Value);
        }

        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> DeleteCart(int id)
        {
           
            var cart =await _cartService.DeleteCart(id);

            if(!cart.IsSuccess)
                return BadRequest(cart.ErrorMessage);

            return Ok(cart.Value);
        }



    }
}
