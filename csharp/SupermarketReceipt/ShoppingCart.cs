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

            var unitPrice = catalog.GetUnitPrice(product);
            var quantityAsInt = (int) TotalQuantityForProduct(product);

            var offer = offers[product];

            return CalculateDiscount(offer, product, quantityAsInt, unitPrice);
        }

        private Discount CalculateDiscount(Offer offer, Product product, int quantityAsInt, double unitPrice)
        {
            return offer.OfferType switch
            {
                SpecialOfferType.TwoForAmount => CalcultateDiscountForTwoForAmount(product, quantityAsInt, offer, unitPrice, TotalQuantityForProduct(product)),
                SpecialOfferType.ThreeForTwo => CalcultateDiscountForThreeForTwoDiscount(product, quantityAsInt, TotalQuantityForProduct(product), unitPrice),
                SpecialOfferType.TenPercentDiscount => CalcultateDiscountForTenPercentDiscount(product, offer, TotalQuantityForProduct(product), unitPrice),
                SpecialOfferType.FiveForAmount => CalcultateDiscountForFivePerAmountDiscount(product, quantityAsInt, unitPrice, TotalQuantityForProduct(product), offer),
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

        private IEnumerable<Product> UniqueItemsInCart() => _items.Select(i=>i.Product).Distinct();

        private double TotalQuantityForProduct(Product product) => _items.Where(i => i.Product == product).Sum(p => p.Quantity);
    }
}