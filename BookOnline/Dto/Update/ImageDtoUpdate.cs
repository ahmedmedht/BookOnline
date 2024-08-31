namespace BookOnline.Dto.Update
{
    public class ImageDtoUpdate
    {
        public Guid Guid { get; set; }
        public IFormFile ImageFile { get; set; }

    }
}
