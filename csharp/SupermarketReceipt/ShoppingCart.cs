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
            var offer = offers[product];

            return offer.CalculateDiscount(product, unitPrice, TotalQuantityForProduct(product), this);
        }

        private IEnumerable<Product> UniqueItemsInCart() => _items.Select(i=>i.Product).Distinct();

        private double TotalQuantityForProduct(Product product) => _items.Where(i => i.Product == product).Sum(p => p.Quantity);
    }
}