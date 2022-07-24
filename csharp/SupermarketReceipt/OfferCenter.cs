using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt;

public class OfferCenter
{
    private readonly Dictionary<Product, IOffer> _offers;
    private readonly List<BundleOffer> _bundleOffers;
    private readonly SupermarketCatalog _catalog;

    public OfferCenter(Dictionary<Product, IOffer> offers, List<BundleOffer> bundleOffers, SupermarketCatalog catalog)
    {
        _offers = offers;
        _bundleOffers = bundleOffers;
        _catalog = catalog;
    }

    public IEnumerable<Discount> IndividualProductDiscountsFor(ShoppingCart shoppingCart) =>
        shoppingCart.UniqueItemsInCart()
            .Where(ProductcurrentlyHasAnOffer)
            .Select(p=> CalculateDiscountFor(p, shoppingCart))
            .Where(d=> d !=null);

    private Discount CalculateDiscountFor(Product product, ShoppingCart shoppingCart)
    {
        var offer = _offers[product];

        return offer.CalculateDiscount(_catalog.GetUnitPrice(product), shoppingCart.TotalQuantityForProduct(product));
    }

    private bool ProductcurrentlyHasAnOffer(Product p) => _offers.ContainsKey(p);

    public IEnumerable<Discount> BundledProductDiscountsFor(ShoppingCart theCart)
    {
        return _bundleOffers.Select(o => o.CalculateDiscount(theCart)).Where(d=> d!=null).ToList();
    }
}