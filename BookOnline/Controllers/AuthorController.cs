
using AutoMapper;
using BookOnline.Dto;
using BookOnline.Model;
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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        


        public AuthorController(IAuthorService authorService,
            IMapper mapper, IImageService imageService)
        {
            _authorService = authorService;
            _mapper = mapper;
            _imageService = imageService;
        }


        [HttpGet("GetAllAuthor")]
        public async Task<IActionResult> GetAllAsync()
        {
            var authors=await _authorService.GetAllAsync();
            if (authors.IsSuccess == false)
                return BadRequest(authors.ErrorMessage);

            return Ok(authors.Value);
        }

        [HttpPost("AddNewAuthor")]
        public async Task<IActionResult> AddAuthor([FromForm] AuthorDto dto)
        {
            

            var author=await _authorService.AddAsync(dto);
            if (author.IsSuccess == false)
                return BadRequest(author.ErrorMessage);

            return Ok(author);

        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateTable([FromForm] AuthorDtoUpdate dto)
        {
            var author =await _authorService.Update(dto);

            if (author.IsSuccess == false) 
                return BadRequest(author.ErrorMessage);

            return Ok(author.Value);
        }

        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            
            var author= await _authorService.DeleteAuthor(id);
            
            if (author.IsSuccess == false)
                return BadRequest(author.ErrorMessage);
            
            return Ok(author);
        }






    }
}
