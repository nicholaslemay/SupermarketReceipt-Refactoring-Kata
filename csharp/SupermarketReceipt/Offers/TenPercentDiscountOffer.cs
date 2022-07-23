namespace SupermarketReceipt.Offers;

public class TenPercentDiscountOffer : Offer
{
    private readonly Product _product;
    private readonly double _argument;
    public TenPercentDiscountOffer(Product product, double argument)
    {
        _product = product;
        _argument = argument;
    }

    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        return new Discount(_product, _argument + "% off", -quantity * unitPrice * _argument / 100.0);
    }
}