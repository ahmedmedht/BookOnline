using System.ComponentModel.DataAnnotations;

namespace BookOnline.Dto
{
    public class AuthorDto //Data Transfer Object
    {
        [MaxLength(100)]
        public string Name { get; set; }
        public DateOnly BrithDayDate { get; set; }
        public IFormFile? ImageAuthor { get; set; }
    }
}
