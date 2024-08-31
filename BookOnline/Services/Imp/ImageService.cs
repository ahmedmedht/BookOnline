
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

        public async Task<Response<ImageInfo>> AddAsync(IFormFile file)
        {
            var saveFile = await SaveImageInPath(file);
            if(!saveFile.IsSuccess) 
                return saveFile;
            try
            {
                await _context.AddAsync(saveFile.Value);

                _context.SaveChanges();
            }
            catch (Exception ex) {
                return new Response<ImageInfo>
                {
                    ErrorMessage = ex.Message,
                    IsSuccess = false
                };
            }
            return new Response<ImageInfo>
            {
                IsSuccess = true,
                Value = saveFile.Value
            };

        }

        public async Task<Response<ImageInfo>> DeleteImage(Guid guid)
        {
            var imageInfo= await GetByIDAsync(guid);
            if (!imageInfo.IsSuccess) 
                return imageInfo;
            var deleteFromPath= await DeleteImageFromPath(guid);
            if (!deleteFromPath.IsSuccess)
                return deleteFromPath;

            //string imagePath = Path.Combine(imageInfo.Value.Path, guid.ToString());
            //if (File.Exists(imagePath))
            //{
            //    try
            //    {
            //        File.Delete(imagePath);
            //    }
            //    catch (Exception ex)
            //    {

            //        imageInfo.ErrorMessage = "Image didn't delete.";
            //        imageInfo.IsSuccess = false;
            //        return imageInfo;
            //    }
            //}
            try
            {
                _context.Remove(imageInfo.Value);
                _context.SaveChanges();
                return imageInfo;
            }

            catch (Exception ex) { 
                imageInfo.ErrorMessage = ex.Message + "Image deleted form path but it didn't delete from database.";
                imageInfo.IsSuccess = false;
                return imageInfo;   
            }
        }

        public async Task<Response<ImageInfo>> GetByIDAsync(Guid guid)
        {
            var imageInfo= new Response<ImageInfo>();
            try
            {
                imageInfo.Value = await _context.Images.SingleOrDefaultAsync(b => b.Id == guid);
                imageInfo.IsSuccess = true;
                if (imageInfo.Value == null)
                {
                    imageInfo.ErrorMessage = "Image not found";
                    imageInfo.IsSuccess = false;
                }
                return imageInfo;
            }catch(Exception ex)
            {
                imageInfo.ErrorMessage = ex.Message;
                imageInfo.IsSuccess = false;
                return imageInfo;
            }
        }

        public Task<Response<IFormFile>> GetImage(Guid id)
        {
            throw new NotImplementedException();
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
                    ErrorMessage = "Max size 5 mb",
                    IsSuccess = false                };
            }
            try
            {
                if (!Path.Exists(_folderPath))
                    Directory.CreateDirectory(_folderPath);
                    Guid guid = Guid.NewGuid();

                string newFileName = guid.ToString() +
                    Path.GetExtension(file.FileName);
                string filePath = Path.Combine(_folderPath, newFileName);

            
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

        public async Task<Response<ImageInfo>> ChangeImage(ImageDtoUpdate dtoUpdate)
        {
            var ImageInfo= await GetByIDAsync(dtoUpdate.Guid);
            if (!ImageInfo.IsSuccess)
                return ImageInfo;

            var deleteImg = await DeleteImage(dtoUpdate.Guid);
            if (!deleteImg.IsSuccess)
                return deleteImg;

            var saveImg = await AddAsync(dtoUpdate.ImageFile);
            if (!saveImg.IsSuccess)
                return saveImg;

            return saveImg;
        }

        public async Task<Response<ImageInfo>> DeleteImageFromPath(Guid guid)
        {
            var imageInfo = await GetByIDAsync(guid);
            if (!imageInfo.IsSuccess)
                return imageInfo;

            string imagePath = Path.Combine(imageInfo.Value.Path, guid.ToString());
            
            try
            {
                File.Delete(imagePath);
                imageInfo.ErrorMessage = "Image deleted";
                return imageInfo;
            }
            catch (Exception ex)
            {

                imageInfo.ErrorMessage = "Image didn't delete.";
                imageInfo.IsSuccess = false;
                return imageInfo;
            }
            
        }
    }
}
