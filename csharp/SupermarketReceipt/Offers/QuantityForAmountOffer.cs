namespace SupermarketReceipt.Offers;

public class QuantityForAmountOffer : IOffer
{
    private readonly Product _product;
    private readonly double _discountedAmount;
    private readonly int _quantityElligibleForDiscount;

    public QuantityForAmountOffer(Product product, int quantityElligibleForDiscount, double discountedAmount)
    {
        _product = product;
        _discountedAmount = discountedAmount;
        _quantityElligibleForDiscount = quantityElligibleForDiscount;
    }

    public Discount CalculateDiscount(double unitPrice, double quantity)
    {
        var quantityAsInt = (int)quantity;
        
        if (quantityAsInt < _quantityElligibleForDiscount)
            return null;
        var numberOfDiscountsToApply = quantityAsInt / _quantityElligibleForDiscount;
        var discountTotal = unitPrice * quantity - (_discountedAmount * numberOfDiscountsToApply + quantityAsInt % _quantityElligibleForDiscount * unitPrice);
        return new Discount(_product, _quantityElligibleForDiscount + " for " + _discountedAmount, -discountTotal);
    }
}