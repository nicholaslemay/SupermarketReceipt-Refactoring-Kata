using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new();

        public List<ProductQuantity> GetItems() => new(_items);

        public void AddSingleItem(Product product) => AddItemQuantity(product, 1.0);

        public void AddItemQuantity(Product product, double quantity) => 
            _items.Add(new ProductQuantity(product, quantity));

        public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, SupermarketCatalog catalog) => 
            receipt.AddDiscounts(AllAvailableDiscountsBasedOn(offers, catalog));

        private IEnumerable<Discount> AllAvailableDiscountsBasedOn(Dictionary<Product, Offer> offers, SupermarketCatalog catalog) => 
            UniqueItemsInCart()
                .Select(p => CalculateDiscountFor(p, offers, catalog))
                .Where(d=> d !=null);

        private Discount CalculateDiscountFor(Product product, Dictionary<Product, Offer> offers, SupermarketCatalog catalog)
        {
            if (!offers.ContainsKey(product)) return null;
            
            Discount discount = null;
            var quantity = TotalQuantityForProduct(product);
            var quantityAsInt = (int) quantity;

            var offer = offers[product];
            var unitPrice = catalog.GetUnitPrice(product);

            var quantityForOfferType = QuantityForOfferType(offer.OfferType);

            var sizeOfBundlesOfOfferType = quantityAsInt / quantityForOfferType;

            return offer.OfferType switch
            {
                SpecialOfferType.TwoForAmount => CalcultateDiscountForTwoForAmount(product, quantityAsInt, offer, quantityForOfferType, unitPrice, quantity),
                SpecialOfferType.ThreeForTwo => CalcultateDiscountForThreeForTwoDiscount(product, quantityAsInt, quantity, unitPrice, sizeOfBundlesOfOfferType, discount),
                SpecialOfferType.TenPercentDiscount => CalcultateDiscountForTenPercentDiscount(product, offer, discount, quantity, unitPrice),
                SpecialOfferType.FiveForAmount => CalcultateDiscountForFivePerAmountDiscount(product, quantityAsInt, unitPrice, quantity, offer, sizeOfBundlesOfOfferType, quantityForOfferType),
                _ => discount
            };
        }

        private static Discount CalcultateDiscountForThreeForTwoDiscount(Product product, int quantityAsInt, double quantity, double unitPrice, int sizeOfBundlesOfOfferType, Discount discount)
        {
            if (quantityAsInt < 3)
                return null;
            var discountAmount = quantity * unitPrice - (sizeOfBundlesOfOfferType * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
            return  new Discount(product, "3 for 2", -discountAmount);
        }

        private static Discount CalcultateDiscountForFivePerAmountDiscount(Product product, int quantityAsInt, double unitPrice, double quantity, Offer offer, int sizeOfBundlesOfOfferType, int quantityForOfferType)
        {
            if (quantityAsInt < 5)
                return null;
                var discountTotal = unitPrice * quantity - (offer.Argument * sizeOfBundlesOfOfferType + quantityAsInt % 5 * unitPrice);
                return new Discount(product, quantityForOfferType + " for " + offer.Argument, -discountTotal);
        }

        private static Discount CalcultateDiscountForTenPercentDiscount(Product product, Offer offer, Discount discount, double quantity, double unitPrice)
        {
            return new Discount(product, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
        }

        private static Discount CalcultateDiscountForTwoForAmount(Product product, int quantityAsInt, Offer offer, int quantityForOfferType, double unitPrice, double quantity)
        {
            if (quantityAsInt < 2)
                return null;
            
            var total = offer.Argument * (quantityAsInt / quantityForOfferType) + quantityAsInt % 2 * unitPrice;
            var discountN = unitPrice * quantity - total;
            return new Discount(product, "2 for " + offer.Argument, -discountN);
        }

        private static int QuantityForOfferType(SpecialOfferType offerType)
        {
            return offerType switch
            {
                SpecialOfferType.ThreeForTwo => 3,
                SpecialOfferType.TwoForAmount => 2,
                SpecialOfferType.FiveForAmount => 5,
                _ => 1
            };
        }

        private IEnumerable<Product> UniqueItemsInCart() => _items.Select(i=>i.Product).Distinct();

        private double TotalQuantityForProduct(Product product) => _items.Where(i => i.Product == product).Sum(p => p.Quantity);
    }
}