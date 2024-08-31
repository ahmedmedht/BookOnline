using AutoMapper;
using BookOnline.Dto;
using BookOnline.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BookOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IBookProductService _bookProductService;


        public ProductController(IBookProductService bookProductService)
        {
            _bookProductService = bookProductService;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _bookProductService.GetAllAsync();
            if (!products.IsSuccess)
                return BadRequest(products.ErrorMessage);

            return Ok(products.Value);
        }
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _bookProductService.GetByIDAsync(id);
            return Ok(product.Value);

        }
        [HttpPost("AddNewProduct")]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductDto dto)
        {
            
            var product=await _bookProductService.AddAsync(dto);
            if (!product.IsSuccess) 
                return BadRequest(product.ErrorMessage);

            return Ok(product.Value);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDtoUpdate dto)
        {
            var product = await _bookProductService.Update(dto);
            if (!product.IsSuccess)
                return BadRequest(product.ErrorMessage);

            return Ok(product.Value);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var product = await _bookProductService.DeleteBook(id);
            if (!product.IsSuccess)
                return BadRequest(product.ErrorMessage);

            return Ok(product.Value);

        }


    }
}
