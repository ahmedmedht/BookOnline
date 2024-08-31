using System.ComponentModel.DataAnnotations;

namespace BookOnline.Dto.Update
{
    public class AuthorDtoUpdate
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public DateOnly? BrithDayDate { get; set; }
        public IFormFile? ImageAuthor { get; set; }
    }
}
