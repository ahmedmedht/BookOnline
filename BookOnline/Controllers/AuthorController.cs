
using AutoMapper;
using BookOnline.Dto;
using BookOnline.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        //private readonly IMapper _mapper;

        private new List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1024 * 1024 * 5;

        public AuthorController(IAuthorService authorService, IMapper mapper)
        {
            _authorService = authorService;
            _mapper = mapper;
           
        }
        [HttpGet("GetAuthor")]
        public async Task<IActionResult> GetAllAsync()
        {
            var res=await _authorService.GetAllAsync();
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromForm] AuthorDto dto)
        {
            var author = new Author()
            {
                BrithDayDate = dto.BrithDayDate,
                Name = dto.Name,

            };
            if (dto.ImageAuthor != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.ImageAuthor.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");

                if (_maxAllowedPosterSize < dto.ImageAuthor.Length)
                    return BadRequest("Max size 1 mb");

                using var dataStream = new MemoryStream();
                await dto.ImageAuthor.CopyToAsync(dataStream);

                author.ImageAuthor = dataStream.ToArray();
            }
            await _authorService.AddAsync(author);
            return Ok(author);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateTable(int id,[FromForm] AuthorDto dto)
        {   
            
            var author= await _authorService.GetByIDAsync(id);
            if (author == null) return BadRequest("Author isn't found");

            author = _mapper.Map<Author>(dto);

            if (dto.ImageAuthor != null)
            {
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.ImageAuthor.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");

                if (_maxAllowedPosterSize < dto.ImageAuthor.Length)
                    return BadRequest("Max size 1 mb");

                using var dataStream = new MemoryStream();
                await dto.ImageAuthor.CopyToAsync(dataStream);

                author.ImageAuthor = dataStream.ToArray();
            }
            _authorService.Update(author);
            return Ok(author);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author= await _authorService.GetByIDAsync(id);
            if (author == null)
                return BadRequest("Author not found");
            if (author.BookDetail.Count() > 0)
                return BadRequest("Please delete the author's books first");
            _authorService.DeleteAuthor(author);
            return Ok(author);
        }






    }
}
