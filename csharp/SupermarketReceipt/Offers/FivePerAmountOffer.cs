namespace SupermarketReceipt.Offers;

public class FivePerAmountOffer : Offer
{
    private readonly Product _product;
    private readonly double _argument;

    public FivePerAmountOffer(Product product, double argument)
    {
        _product = product;
        _argument = argument;
    }

    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        if (quantityAsInt < 5)
            return null;
        var numberOfDiscountsToApply = quantityAsInt / 5;
        var discountTotal = unitPrice * quantity - (_argument * numberOfDiscountsToApply + quantityAsInt % 5 * unitPrice);
        return new Discount(_product, 5 + " for " + _argument, -discountTotal);
    }
}