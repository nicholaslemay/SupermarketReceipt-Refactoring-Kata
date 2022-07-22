namespace SupermarketReceipt;

public class FivePerAmountOffer : Offer
{
    public FivePerAmountOffer(Product product, double argument) : base(product, argument)
    {
    }
        
    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        if (quantityAsInt < 5)
            return null;
        var numberOfDiscountsToApply = quantityAsInt / 5;
        var discountTotal = unitPrice * quantity - (Argument * numberOfDiscountsToApply + quantityAsInt % 5 * unitPrice);
        return new Discount(_product, 5 + " for " + Argument, -discountTotal);
    }
}