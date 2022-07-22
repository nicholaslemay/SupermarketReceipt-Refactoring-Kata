namespace SupermarketReceipt;

public class TenPercentDiscountOffer : Offer
{
    public TenPercentDiscountOffer(Product product, double argument) : base(product, argument)
    {
    }
        
    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        return new Discount(_product, Argument + "% off", -quantity * unitPrice * Argument / 100.0);
    }
}