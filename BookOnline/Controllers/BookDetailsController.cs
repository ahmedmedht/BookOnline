using BookOnline.Services;
using BookOnline.Dto;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookOnline.Services.imp;
using AutoMapper;

namespace BookOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDetailsController : ControllerBase
    {
        private readonly IBookDetailService _bookDetailService;
        private readonly IAuthorService _authorService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public BookDetailsController(IBookDetailService bookDetailService, IAuthorService authorService,IMapper mapper,IImageService imageService)

        {
            _bookDetailService = bookDetailService;
            _authorService = authorService;
            _mapper = mapper;
            _imageService = imageService;
        }




        [HttpGet("GetAllBookInfo")]
        public async Task<IActionResult> GetAll()
        {
            var books=await _bookDetailService.GetAllAsync();
            if (books.IsSuccess == false)
                return BadRequest(books.ErrorMessage);

            return Ok(books.Value);

        }




        [HttpPost("AddNewBookInfo")]
        public async Task<IActionResult> AddBook([FromForm] BookDetailsDto dto)
        {
            var book =await _bookDetailService.AddAsync(dto);
            if (book.IsSuccess == false)
                return BadRequest(book.ErrorMessage);

            return Ok(book.Value);
        }


        [HttpPut("UpdateBookInfo")]
        public async Task<IActionResult> UpdateBookDetails([FromForm] BookDtoUpdate dto)
        {

            var book = await _bookDetailService.Update(dto);
            if (book.IsSuccess == false)
                return BadRequest(book.ErrorMessage);

            return Ok(book.Value);
        }



        [HttpDelete("DeleteBookInfo")]
        public async Task<IActionResult> DeleteBook(int id) { 
        
            var book = await _bookDetailService.DeleteBook(id);
            if (book.IsSuccess == false)
                return BadRequest(book.ErrorMessage);

            return Ok(book.Value);
        }

    }
}
