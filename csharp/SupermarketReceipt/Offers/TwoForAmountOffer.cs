namespace SupermarketReceipt.Offers;

public class TwoForAmountOffer : Offer
{
    private readonly Product _product;
    private readonly double _discountedPrice;
    
    public TwoForAmountOffer(Product product, double discountedPrice)
    {
        _product = product;
        _discountedPrice = discountedPrice;
    }

    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        if (quantityAsInt < 2)
            return null;
            
        var total = _discountedPrice * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
        var discountN = unitPrice * quantity - total;
        return new Discount(_product, "2 for " + _discountedPrice, -discountN);
    }
        
}