namespace SupermarketReceipt
{
    public abstract class Discount
    {
        protected Discount(string description, double discountAmount)
        {
            Description = description;
            DiscountAmount = discountAmount;
        }

        public string Description { get; }
        public double DiscountAmount { get; }
    }

    public class IndividualProductDiscount : Discount
    {
        public IndividualProductDiscount(Product product, string description, double discountAmount) : base($"{description}({product.Name})", discountAmount)
        {
        }
    }
}