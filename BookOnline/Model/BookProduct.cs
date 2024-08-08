namespace BookOnline.Model
{
    public class BookProduct
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public int BookDetailsId { get; set; }
        public BookDetail BookDetail { get; set; }
    }
}
