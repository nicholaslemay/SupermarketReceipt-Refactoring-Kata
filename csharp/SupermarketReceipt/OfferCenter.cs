using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt;

public class OfferCenter
{
    private readonly ShoppingCart _shoppingCart;
    private Dictionary<Product, IOffer> _offers;

    public OfferCenter(ShoppingCart shoppingCart, Dictionary<Product, IOffer> offers)
    {
        _shoppingCart = shoppingCart;
        _offers = offers;
    }

    public IEnumerable<Discount> AllAvailableDiscountsBasedOn(Dictionary<Product, IOffer> offers, SupermarketCatalog catalog) =>
        _shoppingCart.UniqueItemsInCart()
            .Select(p => CalculateDiscountFor(p,catalog))
            .Where(d=> d !=null);

    private Discount CalculateDiscountFor(Product product,  SupermarketCatalog catalog)
    {
        if (!_offers.ContainsKey(product)) return null;

        var unitPrice = catalog.GetUnitPrice(product);
        var offer = _offers[product];

        return offer.CalculateDiscount(unitPrice, _shoppingCart.TotalQuantityForProduct(product));
    }
}