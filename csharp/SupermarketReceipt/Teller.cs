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
        public BundleOffer(double percentage, params Product[] toothbrush)
        {
        }

        public Discount CalculateDiscount(ShoppingCart theCart)
        {
            return null;
        }
    }
}