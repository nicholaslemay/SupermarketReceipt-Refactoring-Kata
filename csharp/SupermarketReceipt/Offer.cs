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
        private Product _product;

        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            _product = product;
        }

        public SpecialOfferType OfferType { get; }
        public double Argument { get; }

        public Discount CalculateDiscount(Product product, double unitPrice, double totalQuantityForProduct, ShoppingCart shoppingCart)
        {
            var quantityAsInt = (int)totalQuantityForProduct;
            return this.OfferType switch
            {
                SpecialOfferType.TwoForAmount => CalcultateDiscountForTwoForAmount(product, quantityAsInt, this, unitPrice, totalQuantityForProduct),
                SpecialOfferType.ThreeForTwo => CalcultateDiscountForThreeForTwoDiscount(product, quantityAsInt, totalQuantityForProduct, unitPrice),
                SpecialOfferType.TenPercentDiscount => CalcultateDiscountForTenPercentDiscount(product, this, totalQuantityForProduct, unitPrice),
                SpecialOfferType.FiveForAmount => CalcultateDiscountForFivePerAmountDiscount(product, quantityAsInt, unitPrice, totalQuantityForProduct, this),
                _ => null
            };
        }
        
        private static Discount CalcultateDiscountForThreeForTwoDiscount(Product product, int quantityAsInt, double quantity, double unitPrice)
        {
            if (quantityAsInt < 3)
                return null;
            var numberOfDiscountsToApply = quantityAsInt / 3;
            var discountAmount = quantity * unitPrice - (numberOfDiscountsToApply * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
            return  new Discount(product, "3 for 2", -discountAmount);
        }

        private static Discount CalcultateDiscountForFivePerAmountDiscount(Product product, int quantityAsInt, double unitPrice, double quantity, Offer offer)
        {
            if (quantityAsInt < 5)
                return null;
            var numberOfDiscountsToApply = quantityAsInt / 5;
            var discountTotal = unitPrice * quantity - (offer.Argument * numberOfDiscountsToApply + quantityAsInt % 5 * unitPrice);
            return new Discount(product, 5 + " for " + offer.Argument, -discountTotal);
        }

        private static Discount CalcultateDiscountForTenPercentDiscount(Product product, Offer offer, double quantity, double unitPrice)
        {
            return new Discount(product, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
        }

        private static Discount CalcultateDiscountForTwoForAmount(Product product, int quantityAsInt, Offer offer, double unitPrice, double quantity)
        {
            if (quantityAsInt < 2)
                return null;
            
            var total = offer.Argument * (quantityAsInt / 2) + quantityAsInt % 2 * unitPrice;
            var discountN = unitPrice * quantity - total;
            return new Discount(product, "2 for " + offer.Argument, -discountN);
        }
    }
}