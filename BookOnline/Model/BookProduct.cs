namespace BookOnline.Model
{
    public class BookProduct
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public double price { get; set; }
        public int BookDetailsId { get; set; }
        public BookDetail BookDetail { get; set; }
    }
}
