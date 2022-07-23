using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt;

public class OfferCenter
{
    private readonly ShoppingCart _shoppingCart;
    private readonly Dictionary<Product, IOffer> _offers;
    private readonly SupermarketCatalog _catalog;

    public OfferCenter(ShoppingCart shoppingCart, Dictionary<Product, IOffer> offers, SupermarketCatalog catalog)
    {
        _shoppingCart = shoppingCart;
        _offers = offers;
        _catalog = catalog;
    }

    public IEnumerable<Discount> AllAvailableDiscountsFor(ShoppingCart shoppingCart) =>
        shoppingCart.UniqueItemsInCart()
            .Select(p=> CalculateDiscountFor(p, shoppingCart))
            .Where(d=> d !=null);

    private Discount CalculateDiscountFor(Product product, ShoppingCart shoppingCart)
    {
        if (!_offers.ContainsKey(product)) return null;

        var unitPrice = _catalog.GetUnitPrice(product);
        var offer = _offers[product];

        return offer.CalculateDiscount(unitPrice, shoppingCart.TotalQuantityForProduct(product));
    }
}