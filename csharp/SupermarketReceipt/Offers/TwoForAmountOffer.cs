namespace SupermarketReceipt.Offers;

public class TwoForAmountOffer : Offer
{
    public TwoForAmountOffer(Product product, double argument) : base(product, argument)
    {
    }
        
    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        if (quantityAsInt < 2)
            return null;
            
        var total = Argument * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
        var discountN = unitPrice * quantity - total;
        return new Discount(_product, "2 for " + Argument, -discountN);
    }
        
}