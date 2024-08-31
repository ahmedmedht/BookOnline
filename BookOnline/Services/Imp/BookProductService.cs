using AutoMapper;

namespace BookOnline.Services.Imp
{
    public class BookProductService : IBookProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookDetailService _bookDetailService;
        private readonly IMapper _mapper;

        public BookProductService(ApplicationDbContext context, IBookDetailService bookDetailService, IMapper mapper)
        {
            _context = context;
            _bookDetailService = bookDetailService;
            _mapper = mapper;
        }

        public async Task<Response<BookProduct>> AddAsync(ProductDto dto)
        {
            var product = new Response<BookProduct>();
            var book = await _bookDetailService.GetByIDAsync(dto.BookDetailsId);
            if (book.IsSuccess == false){
                product.ErrorMessage = book.ErrorMessage;
                product.IsSuccess = false;
                return product;
            }

            product.Value = _mapper.Map<BookProduct>(dto);
            try
            {
                await _context.AddAsync(product.Value);
                _context.SaveChanges();

                product.IsSuccess = true;
                return product;
            }
            catch (Exception ex) { 
                product.ErrorMessage = ex.Message;
                product.IsSuccess = false;
                return product;
            }
            
        }

        public async Task<Response<BookProduct>> DeleteBook(int id)
        {
            var product = await GetByIDAsync(id);
            if (product.IsSuccess == false)
                return product;

            try
            {
                _context.Remove(product);
                _context.SaveChanges();
                product.IsSuccess = true;
                return product;

            }
            catch (Exception ex)
            {
                product.ErrorMessage = ex.Message;
                product.IsSuccess = false;
                return product;
            }
        }

        public async Task<Response<IEnumerable<BookProduct>>> GetAllAsync()
        {
            try
            {
                var products = await _context.Products.Include(b => b.BookDetail).ToListAsync();
                return new Response<IEnumerable<BookProduct>>
                {
                    IsSuccess = true,
                    Value = products
                };
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<BookProduct>>
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<Response<BookProduct>> GetByIDAsync(int id)
        {
            if (id == null)
            {
                return new Response<BookProduct>
                {
                    ErrorMessage = "ID is requird",
                    IsSuccess = false,
                    Value = null
                };
            }
            var res = await _context.Products.Include(a => a.BookDetail).SingleOrDefaultAsync(b => b.Id == id);
            if (res == null)
            {
                return new Response<BookProduct>
                {
                    ErrorMessage = "Product not found",
                    IsSuccess = false,
                    Value = null
                };
            }
            return new Response<BookProduct>
            {
                ErrorMessage = "",
                IsSuccess = true,
                Value = res
            };
        }

        public async Task<Response<BookProduct>> Update(ProductDtoUpdate dto)
        {
            var product = await GetByIDAsync(dto.Id);
            if (product.IsSuccess == false)
                return product;

            if(dto.BookDetailsId != null) { 
            var book = await _bookDetailService.GetByIDAsync(dto.BookDetailsId);
            if (book.IsSuccess == false)
                {
                    product.ErrorMessage = book.ErrorMessage;
                    product.IsSuccess = false;
                    return product;
                }
                product.Value.BookDetailsId = book.Value.Id;
            }
            if (dto.Count != -1) 
                product.Value.Count = dto.Count;
            if (dto.price != -1)
                product.Value.Price = dto.price;

            try
            {
                _context.Update(product);
                _context.SaveChanges();
                product.IsSuccess = true;
                return product;

            }
            catch (Exception ex) { 
                product.ErrorMessage = ex.Message;
                product.IsSuccess = false;
                return product;
            }
        }
    }
}

