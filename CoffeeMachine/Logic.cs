using System;
using System.Data.Odbc;

namespace CoffeeMachine
{
    using System.Collections.Generic;

    public class Logic
    {
        private Dictionary<ProductKind, Product> _products = new Dictionary<ProductKind, Product>
        {
            [ProductKind.Coffee] = new Product { Kind = ProductKind.Coffee, Code = "C", Price = 0.6 },
            [ProductKind.Chocolate] = new Product { Kind = ProductKind.Chocolate, Code = "H", Price = 0.5 },
            [ProductKind.Tea] = new Product { Kind = ProductKind.Tea, Code = "T", Price = 0.4 },
            [ProductKind.OrangeJuice] = new Product { Kind = ProductKind.OrangeJuice , Code = "O", Price = 0.6 , IsCold = true }
        };

        public string Translate(Order order)
        {
            var product = _products[order.ProductKind];

            bool hasNotEnoughForDrink = order.Money < product.Price;
            if (hasNotEnoughForDrink)
            {
                return $"M:Not enough money, missing {Math.Abs(order.Money - product.Price)}";
            }

            var extraHotCode = !product.IsCold && order.ExtraHot ? "h" : string.Empty;

            int stick = order.Sugar >= 1 ? 1 : 0;
            return $"{product.Code}{extraHotCode}:{order.Sugar}:{stick}";
        }

    }
}