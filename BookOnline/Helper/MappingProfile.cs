using AutoMapper;
using BookOnline.Dto;

namespace BookOnline.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Author, AuthorDto>();
            CreateMap<BookDetail, BookDetailsDto>();
            CreateMap<BookProduct, ProductDto>();
        }
    }
}
