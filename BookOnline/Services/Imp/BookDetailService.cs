
using AutoMapper;
using BookOnline.Model;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace BookOnline.Services.imp
{
    public class BookDetailService : IBookDetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorService _authorService;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public BookDetailService(ApplicationDbContext context, IAuthorService authorService, IImageService imageService, IMapper mapper)
        {
            _context = context;
            _authorService = authorService;
            _imageService = imageService;
            _mapper = mapper;
        }


        public async Task<Response<BookDetail>> AddAsync(BookDetailsDto dto)
        {
            var book = new Response<BookDetail> {
                Value=_mapper.Map<BookDetail>(dto)
            };

            var author = await _authorService.GetByIDAsync(dto.AuthorId);
            if (author.IsSuccess == false && author.ErrorMessage.Equals("Author not found"))
                {
                    book.ErrorMessage = author.ErrorMessage;
                    book.IsSuccess = false;
                    return book;
                };

            if (dto.BookImage != null)
            {
                var Image = await _imageService.AddAsync(dto.BookImage);
                if (Image.IsSuccess == false)
                {
                    book.ErrorMessage=Image.ErrorMessage;
                    book.IsSuccess = false;
                    return book;
                }

                book.Value.ImageBookId = Image.Value.Id;
            }
            try
            {
                await _context.AddAsync(book.Value);
                _context.SaveChanges();
                return book;
            }
            catch (Exception ex) { 
                book.ErrorMessage = ex.Message;
                book.IsSuccess = false;
                return book;
            }

        }



        public async Task<Response<BookDetail>> DeleteBook(int id)
        {
            var book = await GetByIDAsync(id);

            if (book.IsSuccess == false)
                return book
                    ;
            try
            {
                _context.Remove(book.Value);
                _context.SaveChanges();
                return book;
            }
            catch (Exception ex)
            {
                book.ErrorMessage = ex.Message;
                book.IsSuccess = false;
                return book;
            }
        }



        public async Task<Response<IEnumerable<BookDetail>>> GetAllAsync()
        {
            try
            {
                var books = await _context.BookDetails.Include(b => b.Author).ToListAsync();
                return new Response<IEnumerable<BookDetail>>
                {
                    IsSuccess = true,
                    Value = books
                };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<BookDetail>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
        


        public async Task<Response<BookDetail>> GetByIDAsync(int? id)
        {
            if (id == 0)
            {
                return new Response<BookDetail>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                };
            }
            var res = await _context.BookDetails.Include(a => a.Author).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<BookDetail>
                {
                    ErrorMessage = "Book not found",
                    IsSuccess = false,
                };
            }
            return new Response<BookDetail>
            {
                ErrorMessage = "",
                IsSuccess = true,
                Value = res
            };
        }



        public async Task<Response<BookDetail>> Update(BookDtoUpdate dto)
        {
            var book = await GetByIDAsync(dto.Id);
            if (book.IsSuccess == false)
                return book;

            var author = await _authorService.GetByIDAsync(dto.AuthorId);
            if (!author.IsSuccess && author.ErrorMessage.Equals("Author not found"))
            {
                book.ErrorMessage = author.ErrorMessage;
                book.IsSuccess = false;
                return book;
            };


            if (author.IsSuccess)
                book.Value.AuthorId = author.Value.Id;
            if (dto.Category != null)
                book.Value.Category = dto.Category;
            if(dto.Description != null)
                book.Value.Description = dto.Description;
            if (dto.Title != null)
                book.Value.Title = dto.Title;
            if (dto.GenreForCategory != null)
                book.Value.GenreForCategory = dto.GenreForCategory;
            if (dto.Rate >= 0)
                book.Value.Rate = dto.Rate;
            if (dto.BookImage != null)
            {
                var image = new Response<ImageInfo>();
                if (book.Value.ImageBookId != null)
                {
                    image = await _imageService.ChangeImage(
                       new ImageDtoUpdate { Guid = book.Value.ImageBookId.Value, ImageFile = dto.BookImage });
                }
                else
                {
                    image = await _imageService.AddAsync(dto.BookImage);
                }
                if (image.IsSuccess == false)
                {
                    book.ErrorMessage = image.ErrorMessage;
                    book.IsSuccess = false;
                    return book;
                }

                book.Value.ImageBookId = image.Value.Id;
            }

            try
            {
                _context.Update(book.Value);
                _context.SaveChanges();
                return book;
            }
            catch (Exception ex) {
                book.ErrorMessage = ex.Message;
                book.IsSuccess = false;
                return book;
            }
            
        }
    }
}
