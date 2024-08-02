namespace BookOnline.Dto
{
    public class ProductDto
    {
        public int Count { get; set; }
        public double price { get; set; } // name in public prop (clean code ) First char is always P (A..Z)
        public int BookDetailsId { get; set; }
    }
}
