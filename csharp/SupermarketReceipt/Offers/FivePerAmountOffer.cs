namespace SupermarketReceipt.Offers;

public class FivePerAmountOffer : Offer
{
    private readonly Product _product;

    public FivePerAmountOffer(Product product, double argument) : base(argument)
    {
        _product = product;
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