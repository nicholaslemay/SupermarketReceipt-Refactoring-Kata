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
        private readonly Product _product;

        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            _product = product;
        }

        private SpecialOfferType OfferType { get; }
        private double Argument { get; }

        public Discount CalculateDiscount(double unitPrice, double totalQuantityForProduct)
        {
            return OfferType switch
            {
                SpecialOfferType.TwoForAmount => CalcultateDiscountForTwoForAmount(unitPrice, totalQuantityForProduct),
                SpecialOfferType.ThreeForTwo => CalcultateDiscountForThreeForTwoDiscount(totalQuantityForProduct, unitPrice),
                SpecialOfferType.TenPercentDiscount => CalcultateDiscountForTenPercentDiscount(totalQuantityForProduct, unitPrice),
                SpecialOfferType.FiveForAmount => CalcultateDiscountForFivePerAmountDiscount(unitPrice, totalQuantityForProduct),
                _ => null
            };
        }
        
        private Discount CalcultateDiscountForThreeForTwoDiscount(double quantity, double unitPrice)
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

        private  Discount CalcultateDiscountForTenPercentDiscount(double quantity, double unitPrice)
        {
            return new Discount(_product, Argument + "% off", -quantity * unitPrice * Argument / 100.0);
        }

        private  Discount CalcultateDiscountForTwoForAmount(double unitPrice, double quantity)
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