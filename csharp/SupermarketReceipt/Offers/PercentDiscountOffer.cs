namespace SupermarketReceipt.Offers;

public class PercentDiscountOffer : Offer
{
    private readonly Product _product;
    private readonly double _percentage;
    
    public PercentDiscountOffer(Product product, double percentage)
    {
        _product = product;
        _percentage = percentage;
    }

    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        return new Discount(_product, _percentage + "% off", -quantity * unitPrice * _percentage / 100.0);
    }
}