namespace BookOnline.Services.Imp
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ImageInfo> AddAsync(ImageInfo image)
        {
            await _context.AddAsync(image);
            _context.SaveChanges();
            return image;
        }

        public ImageInfo DeleteImage(ImageInfo imageInfo)
        {
            _context.Remove(imageInfo);
            _context.SaveChanges();
            return imageInfo;
        }

        public async Task<ImageInfo> GetByIDAsync(Guid guid)
        {
            return await _context.Images.SingleOrDefaultAsync(b => b.Id == guid);
        }

        public ImageInfo UpdateImage(ImageInfo image)
        {
            _context.Update(image);
            _context.SaveChanges();
            return image;
        }
    }
}
