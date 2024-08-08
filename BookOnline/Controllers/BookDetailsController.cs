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
        private readonly IMapper _mapper;

        public BookDetailsController(IBookDetailService bookDetailService, IAuthorService authorService,IMapper mapper)
        {
            _bookDetailService = bookDetailService;
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet("GetAllBookInfo")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookDetailService.GetAllAsync());

        }

        [HttpPost("AddNewBookInfo")]
        public async Task<IActionResult> AddBook([FromForm] BookDetailsDto dto)
        {
            var author = await _authorService.GetByIDAsync(dto.AuthorId);
            if (author == null)
                return BadRequest("Author isn't found");

            var book = _mapper.Map<BookDetail>(dto);
            if (dto.BookImage != null)
            {
                using (var dataStream = new MemoryStream())
                {
                    await dto.BookImage.CopyToAsync(dataStream);
                    //book.BookImage = dataStream.ToArray();
                }
            }
            //_mapper.Map<BookDetail>(dto);

            await _bookDetailService.AddAsync(book);
            //           _authorService.AddBookToAuthor(author, book);
            return Ok(book);
        }
        [HttpGet("GetBookByAuthor")]
        public async Task<IActionResult> GetByAuthor(int authorId)
        {
            return Ok(await _bookDetailService.GetAllAsync(authorId));

        }

        [HttpPut("UpdateBookInfo")]
        public async Task<IActionResult> UpdateBookDetails([FromForm] BookDetailsDto dto)
        {
            var book = await _bookDetailService.GetByIDAsync(dto.Id);
            if (book.IsSuccess == false)
                return BadRequest(book.ErrorMessage);

            var author = await _authorService.GetByIDAsync(dto.AuthorId);
            if (author.IsSuccess == false)
                return BadRequest(author.ErrorMessage);

            book.Value = _mapper.Map<BookDetail>(dto);

            if (dto.BookImage != null)
            {
                using (var dataStream = new MemoryStream())
                {
                    await dto.BookImage.CopyToAsync(dataStream);
                   // book.BookImage = dataStream.ToArray();
                }
            }

            _bookDetailService.Update(book.Value);
            return Ok(book.Value);
        }

        [HttpDelete("DeleteBookInfo")]
        public async Task<IActionResult> DeleteBook(int id) {
        
                var book = await _bookDetailService.GetByIDAsync(id);
                if (book.IsSuccess == false)
                return BadRequest(book.ErrorMessage);
            try
            {
                _bookDetailService.DeleteBook(book.Value);
                return Ok(book.Value);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
                }

            }





    }
}
