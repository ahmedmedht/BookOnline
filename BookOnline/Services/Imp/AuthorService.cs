using AutoMapper;
using BookOnline.Dto.Update;
using BookOnline.Model;
using BookOnline.Services.Imp;

namespace BookOnline.Services.imp

{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public AuthorService(ApplicationDbContext context, IImageService imageService, IMapper mapper)
        {
            _context = context;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<Response<Author>> AddAsync(AuthorDto dto)
        {
            var author = new Response<Author> { 
                Value = _mapper.Map<Author>(dto),
                ErrorMessage="",
                IsSuccess=true
            };
            if (dto.ImageAuthor != null)
            {
                var Image = await _imageService.AddAsync(dto.ImageAuthor);
                if (Image.IsSuccess == false)
                {
                    author.IsSuccess = false;
                    author.ErrorMessage = Image.ErrorMessage;
                    return author;
                }
                author.Value.ImageAuthorId = Image.Value.Id;

            }

            await _context.AddAsync(author);
            _context.SaveChanges();
            return  author;
        }

        public async Task<Response<Author>> DeleteAuthor(int id)
        {
            var author = await GetByIDAsync(id);

            if (author.IsSuccess == false)
                return author;

            if (author.Value.BookDetail.Count() > 0) {
                author.ErrorMessage = "Please delete the author's books first";
                author.IsSuccess = false;
                return author;
            }


            try
            {
                _context.Remove(author.Value);
                _context.SaveChanges();
                return author;
            }
            catch (Exception ex)
            {
                author.ErrorMessage = ex.Message;
                author.IsSuccess = false;
                return author;
            }

        }

        public async Task<Response<IEnumerable<Author>>> GetAllAsync()
        {
            try
            {
                var authors = await _context.Author.Include(b => b.BookDetail).ToListAsync();
                return new Response<IEnumerable<Author>>
                {
                    IsSuccess = true,
                    Value = authors
                };
            }
            catch (Exception ex) {
                return new Response<IEnumerable<Author>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Response<Author>> GetByIDAsync(int? id)
        {
            if (id == null)
            {
                return new Response<Author>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                };
            }
            var res = await _context.Author.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<Author>
                {
                    ErrorMessage = "Author not found",
                    IsSuccess = false,
                };
            }
            return new Response<Author>
            {
                ErrorMessage = "",
                IsSuccess = true,
                Value = res
            };
        }

        public async Task<Response<Author>> Update(AuthorDtoUpdate dto)
        {
            var author = await GetByIDAsync(dto.Id);

            if (author.IsSuccess == false)
                return author;

            if(dto.Name != null)
                author.Value.Name = dto.Name;
            if (dto.BrithDayDate != null)
                author.Value.BrithDayDate = (DateOnly)dto.BrithDayDate;
            if (dto.ImageAuthor != null) {
                var image = new Response<ImageInfo>();
                if (author.Value.ImageAuthorId != null)
                {
                     image = await _imageService.ChangeImage(
                        new ImageDtoUpdate { Guid = author.Value.ImageAuthorId.Value, ImageFile = dto.ImageAuthor });
                }
                else
                {
                     image = await _imageService.AddAsync(dto.ImageAuthor);
                }
                if (!image.IsSuccess)
                {
                    author.ErrorMessage = image.ErrorMessage;
                    author.IsSuccess = false;
                    return author;
                }
                author.Value.ImageAuthorId = image.Value.Id;
            }


            _context.Update(author);
            _context.SaveChanges();
            
            return author;
        }
        
    }
}
