namespace BookOnline.Services
{
    public interface IImageService
    {
        Task<Response<ImageInfo>> SaveImageInPath(IFormFile file);
        Task<ImageInfo> AddAsync(ImageInfo image);
        Task<ImageInfo> GetByIDAsync(Guid guid);
        ImageInfo UpdateImage(ImageInfo image);
        ImageInfo DeleteImage(ImageInfo image);
    }
}
