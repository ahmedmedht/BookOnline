namespace BookOnline.Model
{
    public class Cart
    {
        public int Id { get; set; }
        public double  TotalPrice { get; set; } = 0;
        public List<int>  ProductId { get; set; }
        public ICollection<BookProduct> Product { get; set; }
    }
}
