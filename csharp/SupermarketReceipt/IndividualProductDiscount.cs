using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt
{
    public abstract class Discount
    {
        protected Discount(string description, double discountAmount)
        {
            Description = description;
            DiscountAmount = discountAmount;
        }

        public string Description { get; }
        public double DiscountAmount { get; }
    }

    public class IndividualProductDiscount : Discount
    {
        public IndividualProductDiscount(Product product, string description, double discountAmount) : base($"{description}({product.Name})", discountAmount)
        {
        }
    }
    
    public class BundlesProductsDiscount : Discount
    {
        public BundlesProductsDiscount(List<Product> products, string description, double discountAmount) : base(DescriptionFrom(products, description), discountAmount)
        {
        }

        private static string DescriptionFrom(List<Product> products, string description) => $"{description}({string.Join(",", products.Select(p => p.Name))})";

    }
}