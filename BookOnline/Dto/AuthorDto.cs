using System.ComponentModel.DataAnnotations;

namespace BookOnline.Dto
{
    public class AuthorDto
    {
        public int? Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public DateOnly BrithDayDate { get; set; }
        public IFormFile? ImageAuthor { get; set; }
    }
}
