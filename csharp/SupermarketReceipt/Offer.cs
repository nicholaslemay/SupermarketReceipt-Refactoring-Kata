using static SupermarketReceipt.SpecialOfferType;

namespace SupermarketReceipt
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

        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            _product = product;
        }

        protected SpecialOfferType OfferType { get; }
        protected double Argument { get; }

        public abstract Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct);

    }

    class FivePerAmountOffer : Offer
    {
        public FivePerAmountOffer(Product product, double argument) : base(FiveForAmount, product, argument)
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

    class TenPercentDiscountOffer : Offer
    {
        public TenPercentDiscountOffer(Product product, double argument) : base(TenPercentDiscount, product, argument)
        {
        }
        
        public override Discount CalculateDiscount(double unitPrice, double quantity)
        {
            return new Discount(_product, Argument + "% off", -quantity * unitPrice * Argument / 100.0);
        }
    }

    class ThreeForTwoDiscount : Offer
    {
        public ThreeForTwoDiscount(Product product, double argument) : base(ThreeForTwo, product, argument)
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

    public class TwoForAmountOffer : Offer
    {
        public TwoForAmountOffer(Product product, double argument) : base(TwoForAmount, product, argument)
        {
        }
        
        public override Discount CalculateDiscount(double unitPrice, double quantity)
        {
            var quantityAsInt = (int)quantity;
            if (quantityAsInt < 2)
                return null;
            
            var total = Argument * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
            var discountN = unitPrice * quantity - total;
            return new Discount(_product, "2 for " + Argument, -discountN);
        }
        
    }
}