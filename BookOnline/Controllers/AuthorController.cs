
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
        //private readonly IMapper _mapper;

        private new List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1024 * 1024 * 5;
        private string _folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Images");


        public AuthorController(IAuthorService authorService, IMapper mapper, IImageService imageService)
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
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.ImageAuthor.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");

                if (_maxAllowedPosterSize < dto.ImageAuthor.Length)
                    return BadRequest("Max size 1 mb");

                if (!Path.Exists(_folderPath))
                    Directory.CreateDirectory(_folderPath);
                Guid guid = Guid.NewGuid();

                string newFileName = guid.ToString() + Path.GetExtension(dto.ImageAuthor.FileName);
                string filePath = Path.Combine(_folderPath, newFileName);

                try
                {
                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ImageAuthor.CopyToAsync(stream);
                    }

                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
                await _imageService.AddAsync(new ImageInfo() { Id = guid, Path = filePath });
                author.ImageAuthorId = guid;

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
                if (!_allowedExtenstions.Contains(Path.GetExtension(dto.ImageAuthor.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");

                if (_maxAllowedPosterSize < dto.ImageAuthor.Length)
                    return BadRequest("Max size 1 mb");

                using var dataStream = new MemoryStream();
                await dto.ImageAuthor.CopyToAsync(dataStream);

               // author.ImageAuthor = dataStream.ToArray();
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
            _authorService.DeleteAuthor(author.Value);
            return Ok(author);
        }






    }
}
