namespace SupermarketReceipt.Offers
{
    public interface IOffer
    {
        Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);
    }
}