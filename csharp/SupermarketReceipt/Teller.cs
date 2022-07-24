using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly SupermarketCatalog _catalog;
        private readonly Dictionary<Product, IOffer> _offers = new ();
        private readonly List<BundleOffer> _bundleOffers = new();

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

            var offerCenter = new OfferCenter(_offers, _bundleOffers, _catalog);
            receipt.AddDiscounts(offerCenter.IndividualProductDiscountsFor(theCart));
            receipt.AddDiscounts(offerCenter.BundledProductDiscountsFor(theCart));

            return receipt;
        }

        public void AddBundleOffer(BundleOffer bundleOffer)
        {
            _bundleOffers.Add(bundleOffer);
        }
    }
    
    public class BundleOffer
    {
        private readonly double _percentage;
        private readonly Product[] _products;

        public BundleOffer(double percentage, params Product[] products)
        {
            _percentage = percentage;
            _products = products;
        }

        public Discount CalculateDiscount(ShoppingCart theCart, SupermarketCatalog catalog)
        {
            if(theCart.UniqueItemsInCart().Contains(_products[0]))
                return new(_products[0], _percentage + "% off", -1 * catalog.GetUnitPrice(_products[0]) * _percentage / 100.0);
            return null;
        }
    }
}