namespace FrontToBack.ViewModels
{
    public class ProductReturnVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int ProductCount { get; set; }
    }
}
