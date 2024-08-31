using System.ComponentModel.DataAnnotations;

namespace BookOnline.Dto.Update
{
    public class BookDtoUpdate
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        [MaxLength(3000)]
        public string? Description { get; set; }
        public double Rate { get; set; } = -1;
        [MaxLength(50)]
        public string? Category { get; set; }
        [MaxLength(50)]
        public String? GenreForCategory { get; set; }
        public IFormFile? BookImage { get; set; }
        public int? AuthorId { get; set; }
    }
}
