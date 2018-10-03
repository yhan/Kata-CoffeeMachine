namespace CoffeeMachine
{
    using System.Collections.Generic;

    public enum Product
    {
        Coffee,

        Tea,

        Chocolate
    }

    public class Order
    {
        public Product Product { get; set; }

        public int Sugar { get; set; }
    }

    public class Logic
    {
        private Dictionary<Product, string> _codeProducts = new Dictionary<Product, string>
                                                                {
                                                                    [Product.Coffee] = "C",
                                                                    [Product.Tea] = "T",
                                                                    [Product.Chocolate] = "H"
                                                                };

        public string Translate(Order order)
        {
            string codeProduct = _codeProducts[order.Product];

            int stick = order.Sugar >= 1 ? 1 : 0;
            return $"{codeProduct}:{order.Sugar}:{stick}";
        }
    }
}