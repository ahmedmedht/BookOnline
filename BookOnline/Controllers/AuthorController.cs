
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
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        private readonly List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private readonly long _maxAllowedPosterSize = 1024 * 1024 * 5;
        private readonly string _folderPath = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Images");


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
            var res=await _authorService.GetAllAsync();
            return Ok(res);
        }

        [HttpPost("AddNewAuthor")]
        public async Task<IActionResult> AddAuthor([FromForm] AuthorDto dto)
        {
            var author = new Author()
            {
                BrithDayDate = dto.BrithDayDate,
                Name = dto.Name,

            };
            if (dto.ImageAuthor != null)
            {
                var Image = await _imageService.SaveImageInPath(dto.ImageAuthor);
                if (Image.IsSuccess == false) {
                    return BadRequest(Image.ErrorMessage);
                }
                
                await _imageService.AddAsync(Image.Value);
                author.ImageAuthorId = Image.Value.Id;

            }

            await _authorService.AddAsync(author);
            return Ok(author);

        }

        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult> UpdateTable([FromForm] AuthorDto dto)
        {
            
            var author = await _authorService.GetByIDAsync(dto.Id);
            if (author.IsSuccess == false ) 
                    return BadRequest(author.ErrorMessage);
        
            author.Value = _mapper.Map<Author>(dto);

            if (dto.ImageAuthor != null)
            {
                var Image = await _imageService.SaveImageInPath(dto.ImageAuthor);
                if (Image.IsSuccess == false)
                {
                    return BadRequest(Image.ErrorMessage);
                }

                await _imageService.AddAsync(Image.Value);
                author.Value.ImageAuthorId = Image.Value.Id;

            }
            _authorService.Update(author.Value);
            return Ok(author.Value);
        }

        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author= await _authorService.GetByIDAsync(id);
            if (author.IsSuccess == false)
                return BadRequest(author.ErrorMessage);
            if (author.Value.BookDetail.Count() > 0)
                return BadRequest("Please delete the author's books first");
            try
            {
                _authorService.DeleteAuthor(author.Value);

            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
            return Ok(author);
        }






    }
}
