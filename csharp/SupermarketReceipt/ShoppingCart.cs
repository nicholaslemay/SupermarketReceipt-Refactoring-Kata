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

        public IEnumerable<Product> UniqueItemsInCart() => _items.Select(i=>i.Product).Distinct();
        public double TotalQuantityForProduct(Product product) => _items.Where(i => i.Product == product).Sum(p => p.Quantity);
    }
}