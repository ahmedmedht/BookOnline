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
        private readonly IBookDetailService _bookDetailService;
        private readonly IMapper _mapper;

        public ProductController(IBookProductService bookProductService, IBookDetailService bookDetailService
            , IMapper mapper)
        {
            _bookProductService = bookProductService;
            _bookDetailService = bookDetailService;
            _mapper = mapper;
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _bookProductService.GetAllAsync();
            return Ok(products);

        }
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _bookProductService.GetByIDAsync(id);
            return Ok(product);

        }
        [HttpPost("AddNewProduct")]
        public async Task<IActionResult> AddNewProduct([FromForm] ProductDto dto)
        {
            var book = await _bookDetailService.GetByIDAsync(dto.BookDetailsId);
            if (book == null) {
                return BadRequest("Book isn't found");
            }
            var product = _mapper.Map<BookProduct>(dto);
            await _bookProductService.AddAsync(product);
            return Ok(product);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id,[FromForm] ProductDto dto)
        {
            var product = await _bookProductService.GetByIDAsync(id);
            if (product == null)
            {
                return BadRequest("Product isn't found");
            }
            var book = await _bookDetailService.GetByIDAsync(dto.BookDetailsId);
            if (book == null)
            {
                return BadRequest("New Book isn't found");
            }
            product=_mapper.Map<BookProduct>(dto);

            await _bookProductService.AddAsync(product);
            return Ok(product);
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var product = await _bookProductService.GetByIDAsync(id);
            if (product == null)
            {
                return BadRequest("Product isn't found");
            }
            
            _bookProductService.DeleteBook(product);
            return Ok(product);

        }


    }
}
