namespace BookOnline.Dto.Update
{
    public class ProductDtoUpdate
    {
        public int Id { get; set; }
        public int Count { get; set; } = -1;
        public double price { get; set; } = -1;
        public int? BookDetailsId { get; set; }
    }
}
