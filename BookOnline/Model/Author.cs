using System.ComponentModel.DataAnnotations;

namespace BookOnline.Model
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public DateOnly BrithDayDate { get; set; }
        public byte[]?  ImageAuthor { get; set; } // not saving images as binary in db 
        public ImageInfo  AutorImage { get; set; }
        public ICollection<BookDetail> BookDetail { get; set; }
    }

    /*
     // new table image information 
     // image ID 
     // image path 
     */
    public class ImageInfo
    {
         public int Id { get; set; }    
         public string Path { get; set; }
    }
}