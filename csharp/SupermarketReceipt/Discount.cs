namespace SupermarketReceipt
{
    public class Discount
    {
        private string _description;

        public Discount(Product product, string description, double discountAmount)
        {
            _description = description;
            Product = product;
            DiscountAmount = discountAmount;
        }

        public string Description => $"{_description}({Product.Name})"; 
        public double DiscountAmount { get; }
        public Product Product { get; }
    }
}