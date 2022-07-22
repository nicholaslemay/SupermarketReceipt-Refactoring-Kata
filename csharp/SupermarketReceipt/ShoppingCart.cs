using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new();


        public List<ProductQuantity> GetItems() => new(_items);

        public void AddSingleItem(Product product) => AddItemQuantity(product, 1.0);


        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));
        }

        public void HandleOffers(Receipt receipt, Dictionary<Product, Offer> offers, SupermarketCatalog catalog)
        {
            
            foreach (var product in UniqueItemsInCart())
            {
                var quantity = TotalQuantityForProduct(product);
                var discount = CalculateDiscountFor(offers, catalog, product, quantity);
                if (discount != null)
                    receipt.AddDiscount(discount);
                
            }
        }

        private static Discount CalculateDiscountFor(Dictionary<Product, Offer> offers, SupermarketCatalog catalog, Product product, double quantity)
        {
            Discount discount = null;
            var quantityAsInt = (int) quantity;
            if (offers.ContainsKey(product))
            {
                var offer = offers[product];
                var unitPrice = catalog.GetUnitPrice(product);

                var x = 1;
                if (offer.OfferType == SpecialOfferType.ThreeForTwo)
                {
                    x = 3;
                }
                else if (offer.OfferType == SpecialOfferType.TwoForAmount)
                {
                    x = 2;
                    if (quantityAsInt >= 2)
                    {
                        var total = offer.Argument * (quantityAsInt / x) + quantityAsInt % 2 * unitPrice;
                        var discountN = unitPrice * quantity - total;
                        discount = new Discount(product, "2 for " + offer.Argument, -discountN);
                    }
                }

                if (offer.OfferType == SpecialOfferType.FiveForAmount) x = 5;
                var numberOfXs = quantityAsInt / x;
                if (offer.OfferType == SpecialOfferType.ThreeForTwo && quantityAsInt > 2)
                {
                    var discountAmount = quantity * unitPrice - (numberOfXs * 2 * unitPrice + quantityAsInt % 3 * unitPrice);
                    discount = new Discount(product, "3 for 2", -discountAmount);
                }

                if (offer.OfferType == SpecialOfferType.TenPercentDiscount) discount = new Discount(product, offer.Argument + "% off", -quantity * unitPrice * offer.Argument / 100.0);
                if (offer.OfferType == SpecialOfferType.FiveForAmount && quantityAsInt >= 5)
                {
                    var discountTotal = unitPrice * quantity - (offer.Argument * numberOfXs + quantityAsInt % 5 * unitPrice);
                    discount = new Discount(product, x + " for " + offer.Argument, -discountTotal);
                }
            }

            return discount;
        }

        private IEnumerable<Product> UniqueItemsInCart()
        {
            return _items.Select(i=>i.Product).Distinct();
        }

        private double TotalQuantityForProduct(Product product) => _items.Where(i => i.Product == product).Sum(p => p.Quantity);
    }
}