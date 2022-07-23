namespace SupermarketReceipt.Offers;

public class TwoForAmountOffer : Offer
{
    private readonly Product _product;
    private readonly double _argument;
    
    public TwoForAmountOffer(Product product, double argument)
    {
        _product = product;
        _argument = argument;
    }


    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        if (quantityAsInt < 2)
            return null;
            
        var total = _argument * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
        var discountN = unitPrice * quantity - total;
        return new Discount(_product, "2 for " + _argument, -discountN);
    }
        
}