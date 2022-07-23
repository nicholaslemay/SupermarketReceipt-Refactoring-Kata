namespace SupermarketReceipt.Offers;

public class PercentDiscountOffer : IOffer
{
    private readonly Product _product;
    private readonly double _percentage;
    
    public PercentDiscountOffer(Product product, double percentage)
    {
        _product = product;
        _percentage = percentage;
    }

    public Discount CalculateDiscount(double unitPrice, double quantity) => 
        new(_product, _percentage + "% off", -quantity * unitPrice * _percentage / 100.0);
}