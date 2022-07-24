namespace SupermarketReceipt.Offers
{
    public interface IOffer
    {
        IDiscount CalculateDiscount(double unitPrice, double totalQuantityForProduct);
    }
}