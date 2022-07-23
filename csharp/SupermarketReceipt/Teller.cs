using System.Collections.Generic;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly SupermarketCatalog _catalog;
        private readonly Dictionary<Product, IOffer> _offers = new ();

        public Teller(SupermarketCatalog catalog) => _catalog = catalog;

        public void AddSpecialOffer(Product product, IOffer offer) => _offers[product] = offer;

        public Receipt ChecksOutArticlesFrom(ShoppingCart theCart)
        {
            var receipt = new Receipt();
            var productQuantities = theCart.GetItems();
            foreach (var pq in productQuantities)
            {
                var p = pq.Product;
                var quantity = pq.Quantity;
                var unitPrice = _catalog.GetUnitPrice(p);
                var price = quantity * unitPrice;
                receipt.AddProduct(p, quantity, unitPrice, price);
            }

            receipt.AddDiscounts(theCart.AllAvailableDiscountsBasedOn(_offers, _catalog));

            return receipt;
        }
    }
}