namespace BookOnline.Dto
{
    public class ProductDto
    {
        public int? Id { get; set; }
        public int Count { get; set; }
        public double price { get; set; }
        public int BookDetailsId { get; set; }
    }
}
