using static SupermarketReceipt.Offers.SpecialOfferType;

namespace SupermarketReceipt.Offers
{
    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount
    }

    public static class OfferFactory
    {
        public static IOffer Build(SpecialOfferType offerType, Product product, double argument)
        {
            return offerType switch
            {
                TwoForAmount => new TwoForAmountOffer(product, argument),
                ThreeForTwo => new ThreeForTwoDiscount(product),
                TenPercentDiscount => new PercentDiscountOffer(product, argument),
                FiveForAmount => new FivePerAmountOffer(product, argument),
                _ => null
            };
        }
    }

    public interface IOffer
    {
        Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);
    }

    public abstract class Offer : IOffer
    {
        public abstract Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);
    }
}