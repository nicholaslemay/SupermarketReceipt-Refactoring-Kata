namespace SupermarketReceipt.Offers;

public class TenPercentDiscountOffer : Offer
{
    private readonly Product _product;

    public TenPercentDiscountOffer(Product product, double argument) : base(argument)
    {
        _product = product;
    }
        
    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        return new Discount(_product, Argument + "% off", -quantity * unitPrice * Argument / 100.0);
    }
}