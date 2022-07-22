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

            if (offer.OfferType == SpecialOfferType.TwoForAmount)
            {
                discount = CalcultateDiscountForTwoForAmount(product, quantityAsInt, offer, quantityForOfferType, unitPrice, quantity);
            }

            var sizeOfBundlesOfOfferType = quantityAsInt / quantityForOfferType;
            if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
            {
                var discountAmount = quantity * unitPrice - (sizeOfBundlesOfOfferType * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                discount = new Discount(product, "3 for 2", -discountAmount);
            }

            if (offer.OfferType == SpecialOfferType.TenPercentDiscount)
            {
                discount = new Discount(product, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);    
            }
            
            if (offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
            {
                var discountTotal = unitPrice * quantity - (offer.Argument * sizeOfBundlesOfOfferType + quantityAsInt % 5 * unitPrice);
                discount = new Discount(product, quantityForOfferType + " for " + offer.Argument, -discountTotal);
            }

            return discount;
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