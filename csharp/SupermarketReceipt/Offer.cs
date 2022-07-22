namespace SupermarketReceipt
{
    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount
    }

    public class Offer
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

        public Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct)
        {
            return OfferType switch
            {
                SpecialOfferType.TwoForAmount => new TwoForAmountOffer(OfferType, _product, Argument).CalcultateDiscountForTwoForAmount(unitPrice, totalQuantityForProduct),
                SpecialOfferType.ThreeForTwo => CalcultateDiscountForThreeForTwoDiscount(unitPrice, totalQuantityForProduct),
                SpecialOfferType.TenPercentDiscount => CalcultateDiscountForTenPercentDiscount(unitPrice, totalQuantityForProduct),
                SpecialOfferType.FiveForAmount => CalcultateDiscountForFivePerAmountDiscount(unitPrice, totalQuantityForProduct),
                _ => null
            };
        }
        
        public Discount CalcultateDiscountForThreeForTwoDiscount(double unitPrice, double quantity)
        {
            var quantityAsInt = (int)quantity;
            if (quantityAsInt < 3)
                return null;
            var numberOfDiscountsToApply = quantityAsInt / 3;
            var discountAmount = quantity * unitPrice - (numberOfDiscountsToApply * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
            return new Discount(_product, "3 for 2", -discountAmount);
        }

        private Discount CalcultateDiscountForFivePerAmountDiscount(double unitPrice, double quantity)
        {
            var quantityAsInt = (int)quantity;
            if (quantityAsInt < 5)
                return null;
            var numberOfDiscountsToApply = quantityAsInt / 5;
            var discountTotal = unitPrice * quantity - (Argument * numberOfDiscountsToApply + quantityAsInt % 5 * unitPrice);
            return new Discount(_product, 5 + " for " + Argument, -discountTotal);
        }

        private Discount CalcultateDiscountForTenPercentDiscount(double unitPrice, double quantity)
        {
            return new Discount(_product, Argument + "% off", -quantity * unitPrice * Argument / 100.0);
        }

        public  Discount CalcultateDiscountForTwoForAmount(double unitPrice, double quantity)
        {
            var quantityAsInt = (int)quantity;
            if (quantityAsInt < 2)
                return null;
            
            var total = Argument * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
            var discountN = unitPrice * quantity - total;
            return new Discount(_product, "2 for " + Argument, -discountN);
        }
    }

    public class TwoForAmountOffer : Offer
    {
        public TwoForAmountOffer(SpecialOfferType offerType, Product product, double argument) : base(offerType, product, argument)
        {
        }
        
        public  Discount CalcultateDiscountForTwoForAmount(double unitPrice, double quantity)
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