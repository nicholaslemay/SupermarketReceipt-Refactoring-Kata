namespace SupermarketReceipt.Offers;

public class ThreeForTwoDiscount : Offer
{
    public ThreeForTwoDiscount(Product product, double argument) : base(product, argument)
    {
    }
    public override Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        if (quantityAsInt < 3)
            return null;
        var numberOfDiscountsToApply = quantityAsInt / 3;
        var discountAmount = quantity * unitPrice - (numberOfDiscountsToApply * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
        return new Discount(_product, "3 for 2", -discountAmount);
    }
}