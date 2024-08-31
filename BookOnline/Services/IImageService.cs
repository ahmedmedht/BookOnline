namespace BookOnline.Services
{
    public interface IImageService
    {
        Task<Response<ImageInfo>> SaveImageInPath(IFormFile file);
        Task<Response<ImageInfo>> AddAsync(IFormFile file);
        Task<Response<ImageInfo>> GetByIDAsync(Guid guid);

        Task<Response<ImageInfo>> DeleteImageFromPath(Guid guid);
        Task<Response<ImageInfo>> DeleteImage(Guid guid);

        Task<Response<ImageInfo>> ChangeImage(ImageDtoUpdate dtoUpdate);
        Task<Response<IFormFile>> GetImage(Guid id); 
    }
}
