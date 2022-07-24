namespace SupermarketReceipt
{
    public class Discount
    {
        private readonly string _description;
        private readonly Product _product;
        
        public Discount(Product product, string description, double discountAmount)
        {
            _description = description;
            _product = product;
            DiscountAmount = discountAmount;
        }

        public string Description => $"{_description}({_product.Name})"; 
        public double DiscountAmount { get; }
    }
}