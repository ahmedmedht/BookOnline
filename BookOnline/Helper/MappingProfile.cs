using AutoMapper;
using BookOnline.Dto;

namespace BookOnline.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();


            CreateMap<BookDetail, BookDetailsDto>();
            CreateMap<BookDetailsDto, BookDetail>();

            CreateMap<BookProduct, ProductDto>();
        }
    }
}
