using System.Collections.Generic;
using System.Linq;
using SupermarketReceipt.Offers;

namespace SupermarketReceipt;

public class OfferCenter
{
    private readonly ShoppingCart _shoppingCart;

    public OfferCenter(ShoppingCart shoppingCart)
    {
        _shoppingCart = shoppingCart;
    }

    public IEnumerable<Discount> AllAvailableDiscountsBasedOn(Dictionary<Product, IOffer> offers, SupermarketCatalog catalog) =>
        _shoppingCart.UniqueItemsInCart()
            .Select(p => CalculateDiscountFor(p, offers, catalog))
            .Where(d=> d !=null);

    private Discount CalculateDiscountFor(Product product, Dictionary<Product, IOffer> offers, SupermarketCatalog catalog)
    {
        if (!offers.ContainsKey(product)) return null;

        var unitPrice = catalog.GetUnitPrice(product);
        var offer = offers[product];

        return offer.CalculateDiscount(unitPrice, _shoppingCart.TotalQuantityForProduct(product));
    }
}