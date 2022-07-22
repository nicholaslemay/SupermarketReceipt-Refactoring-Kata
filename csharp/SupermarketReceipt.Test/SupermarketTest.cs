
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace SupermarketReceipt.Test
{
    [UsesVerify]
    public class SupermarketTest
    {
        private readonly SupermarketCatalog _catalog;
        private readonly Teller _teller;
        private readonly ShoppingCart _theCart;
        private readonly Product _toothbrush;
        private readonly Product _rice;
        private readonly Product _apples;
        private readonly Product _cherryTomatoes;
        
        public SupermarketTest()
        {
            _catalog = new FakeCatalog();
            _teller = new Teller(_catalog);
            _theCart = new ShoppingCart();

            _toothbrush = new Product("toothbrush", ProductUnit.Each);
            _catalog.AddProduct(_toothbrush, 0.99);
            _rice = new Product("rice", ProductUnit.Each);
            _catalog.AddProduct(_rice, 2.99);
            _apples = new Product("apples", ProductUnit.Kilo);
            _catalog.AddProduct(_apples, 1.99);
            _cherryTomatoes = new Product("cherry tomato box", ProductUnit.Each);
            _catalog.AddProduct(_cherryTomatoes, 0.69);

        }
        
        [Fact]
        public Task an_empty_shopping_cart_should_cost_nothing()
        {
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task one_normal_item()
        {
            _theCart.AddItem(_toothbrush);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task two_normal_items()
        {
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_rice);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task buy_two_get_one_free()
        {
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, _toothbrush, _catalog.GetUnitPrice(_toothbrush));
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task buy_five_get_one_free()
        {
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _theCart.AddItem(_toothbrush);
            _teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, _toothbrush, _catalog.GetUnitPrice(_toothbrush));
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task loose_weight_product()
        {
            _theCart.AddItemQuantity(_apples, .5);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task percent_discount()
        {
            _theCart.AddItem(_rice);
            _teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, _rice, 10.0);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task xForY_discount()
        {
            _theCart.AddItem(_cherryTomatoes);
            _theCart.AddItem(_cherryTomatoes);
            _teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, _cherryTomatoes, .99);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task FiveForY_discount()
        {
            _theCart.AddItemQuantity(_apples, 5);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _apples, 6.99);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task FiveForY_discount_withSix()
        {
            _theCart.AddItemQuantity(_apples, 6);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _apples, 6.99);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task FiveForY_discount_withSixteen()
        {
            _theCart.AddItemQuantity(_apples, 16);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _apples, 6.99);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }

        [Fact]
        public Task FiveForY_discount_withFour()
        {
            _theCart.AddItemQuantity(_apples, 4);
            _teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, _apples, 6.99);
            var receipt = _teller.ChecksOutArticlesFrom(_theCart);
            return Verifier.Verify(new ReceiptPrinter(40).PrintReceipt(receipt));
        }
    }
}