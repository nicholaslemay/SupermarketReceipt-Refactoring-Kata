using System.Linq;

namespace SupermarketReceipt;

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
        var minimumQtyPurchasedOfEachProduct = _products.Select(theCart.TotalQuantityForProduct).Min();
        var numberOfbundlesToApply = minimumQtyPurchasedOfEachProduct;
            
        if (AllItemsInBundlePurchased(minimumQtyPurchasedOfEachProduct))
            return new BundlesProductsDiscount(_products.ToList(), Description(numberOfbundlesToApply), DiscountAmount(catalog, numberOfbundlesToApply));
            
        return null;
    }

    private string Description(double numberOfbundlesToApply) => 
        $"{numberOfbundlesToApply} * {_percentage} % off ";

    private double DiscountAmount(SupermarketCatalog catalog, double numberOfbundlesToApply) => 
        -TotalCostOfSingleBundle(catalog) * numberOfbundlesToApply * _percentage / 100.0;

    private static bool AllItemsInBundlePurchased(double min) => min > 0;

    private double TotalCostOfSingleBundle(SupermarketCatalog catalog) => _products.Sum(catalog.GetUnitPrice);
}