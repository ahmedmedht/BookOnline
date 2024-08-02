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
        public String? Genre { get; set; }
        //public Byte[]? BookImage { get; set; }
        public int BookImageId { get; set; }
        public ImageInfo BookImage { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }

    }
}
