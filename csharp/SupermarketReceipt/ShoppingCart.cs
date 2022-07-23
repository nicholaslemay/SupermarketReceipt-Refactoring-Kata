using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new();

        public List<ProductQuantity> GetItems() => new(_items);

        public void AddSingleItem(Product product) => AddItemQuantity(product, 1.0);

        public void AddItemQuantity(Product product, double quantity) => 
            _items.Add(new ProductQuantity(product, quantity));

        public void HandleOffers(Receipt receipt, Dictionary<Product, IOffer> offers, SupermarketCatalog catalog) => 
            receipt.AddDiscounts(AllAvailableDiscountsBasedOn(offers, catalog));

        public IEnumerable<Discount> AllAvailableDiscountsBasedOn(Dictionary<Product, IOffer> offers, SupermarketCatalog catalog) => 
            UniqueItemsInCart()
                .Select(p => CalculateDiscountFor(p, offers, catalog))
                .Where(d=> d !=null);

        private IEnumerable<Product> UniqueItemsInCart() => _items.Select(i=>i.Product).Distinct();

        private Discount CalculateDiscountFor(Product product, Dictionary<Product, IOffer> offers, SupermarketCatalog catalog)
        {
            if (!offers.ContainsKey(product)) return null;

            var unitPrice = catalog.GetUnitPrice(product);
            var offer = offers[product];

            return offer.CalculateDiscount(unitPrice, TotalQuantityForProduct(product));
        }
        
        private double TotalQuantityForProduct(Product product) => _items.Where(i => i.Product == product).Sum(p => p.Quantity);
    }
}