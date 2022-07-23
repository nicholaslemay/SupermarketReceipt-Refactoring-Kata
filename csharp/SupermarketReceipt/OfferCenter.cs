using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt;

public class OfferCenter
{
    private readonly Dictionary<Product, IOffer> _offers;
    private readonly SupermarketCatalog _catalog;

    public OfferCenter(Dictionary<Product, IOffer> offers, SupermarketCatalog catalog)
    {
        _offers = offers;
        _catalog = catalog;
    }

    public IEnumerable<Discount> AllAvailableDiscountsFor(ShoppingCart shoppingCart) =>
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
}