
using Newtonsoft.Json.Linq;

namespace BookOnline.Services.Imp
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;

        private readonly List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private readonly long _maxAllowedPosterSize = 1024 * 1024 * 5;
        private readonly string _folderPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "Images");

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

        public async Task<Response<ImageInfo>> SaveImageInPath(IFormFile file)
        {
            if (!_allowedExtenstions.Contains(Path.
                                GetExtension(file.FileName).ToLower()))
            {
                return new Response<ImageInfo>
                {
                    ErrorMessage = "Only .png and .jpg images are allowed",
                    IsSuccess = false
                };
            }
            if (_maxAllowedPosterSize < file.Length)
            {
                return new Response<ImageInfo>
                {
                    ErrorMessage = "Max size 1 mb",
                    IsSuccess = false                };
            }
            

            if (!Path.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);
            Guid guid = Guid.NewGuid();

            string newFileName = guid.ToString() +
                Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_folderPath, newFileName);

            try
            {
                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return new Response<ImageInfo>

                {
                    ErrorMessage = "Image saved in path",
                    IsSuccess = true,
                    Value = new ImageInfo()
                    {
                        Id = guid,
                        Path = filePath
                    }
                };

            }
            catch (Exception ex)
            {
                return new Response<ImageInfo>

                {
                    ErrorMessage = $"Internal server error: {ex.Message}",
                    IsSuccess = true                };
            }
        }

        public ImageInfo UpdateImage(ImageInfo image)
        {
            _context.Update(image);
            _context.SaveChanges();
            return image;
        }

    }
}
