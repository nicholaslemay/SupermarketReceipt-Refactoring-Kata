namespace SupermarketReceipt.Offers
{
    public interface IOffer
    {
        Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);
    }

    public abstract class Offer : IOffer
    {
        public abstract Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);
    }
}