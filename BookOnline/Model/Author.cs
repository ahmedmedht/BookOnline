using System.ComponentModel.DataAnnotations;

namespace BookOnline.Model
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public DateOnly BrithDayDate { get; set; }
        public Guid? ImageAuthorId { get; set; }
        public ImageInfo?  ImageAuthor { get; set; }
        public ICollection<BookDetail> BookDetail { get; set; }
    }
}