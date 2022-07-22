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
        public static Offer Build(SpecialOfferType offerType, Product product, double argument)
        {
            return offerType switch
            {
                TwoForAmount => new TwoForAmountOffer(product, argument),
                ThreeForTwo => new ThreeForTwoDiscount(product, argument),
                TenPercentDiscount => new TenPercentDiscountOffer(product, argument),
                FiveForAmount => new FivePerAmountOffer(product, argument),
                _ => null
            };
        }
    }

    public abstract class Offer
    {
        protected readonly Product _product;

        protected Offer(Product product, double argument)
        {
            Argument = argument;
            _product = product;
        }

        protected double Argument { get; }

        public abstract Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);

    }
}