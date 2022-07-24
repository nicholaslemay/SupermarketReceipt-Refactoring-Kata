using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Receipt
    {
        private readonly List<IDiscount> _discounts = new();
        private readonly List<ReceiptItem> _items = new();

        public double GetTotalPrice()
        {
            var total = 0.0;
            foreach (var item in _items) total += item.TotalPrice;
            foreach (var discount in _discounts) total += discount.DiscountAmount;
            return total;
        }

        public void AddProduct(Product p, double quantity, double price, double totalPrice)
        {
            _items.Add(new ReceiptItem(p, quantity, price, totalPrice));
        }

        public List<ReceiptItem> GetItems()
        {
            return new List<ReceiptItem>(_items);
        }

        public void AddDiscount(IDiscount discount)
        {
            _discounts.Add(discount);
        }

        public List<IDiscount> GetDiscounts()
        {
            return _discounts;
        }

        public void AddDiscounts(IEnumerable<IDiscount> discounts)
        {
            _discounts.AddRange(discounts);     
        }
    }

    public class ReceiptItem
    {
        public ReceiptItem(Product p, double quantity, double price, double totalPrice)
        {
            Product = p;
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
        }

        public Product Product { get; }
        public double Price { get; }
        public double TotalPrice { get; }
        public double Quantity { get; }
    }
}