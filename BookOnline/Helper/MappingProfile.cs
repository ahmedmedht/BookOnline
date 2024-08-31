using AutoMapper;
using BookOnline.Dto;

namespace BookOnline.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Author, AuthorDtoUpdate>();
            CreateMap<AuthorDtoUpdate, Author>();


            CreateMap<BookDetail, BookDetailsDto>();
            CreateMap<BookDetailsDto, BookDetail>()
                 .ForMember(a => a.Id, opt => opt.Ignore())
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            

            CreateMap<BookProduct, ProductDto>();
        }
    }
}
