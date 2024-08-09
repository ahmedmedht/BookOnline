using System.ComponentModel.DataAnnotations;

namespace BookOnline.Model
{
    public class BookDetail
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(3000)]
        public string Description { get; set; }
        public double Rate { get; set; }
        [MaxLength (50)]
        public string Category { get; set; }
        [MaxLength (50)]
        public String? GenreForCategory { get; set; }
        public Guid? ImageBookId { get; set; }
        public ImageInfo? ImageBook { get; set; }
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

    }
}
