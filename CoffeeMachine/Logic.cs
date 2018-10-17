namespace CoffeeMachine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class Logic
    {
        private Dictionary<DateTime, List<Order>> _orders = new Dictionary<DateTime, List<Order>>();

        private Dictionary<ProductKind, Product> _products = new Dictionary<ProductKind, Product>
                                                                 {
                                                                     [ProductKind.Coffee] =
                                                                         new Product
                                                                             {
                                                                                 Kind = ProductKind.Coffee,
                                                                                 Code = "C",
                                                                                 Price = 0.6,
                                                                                 Name = "Coffee"
                                                                             },
                                                                     [ProductKind.Chocolate] =
                                                                         new Product
                                                                             {
                                                                                 Kind = ProductKind.Chocolate,
                                                                                 Code = "H",
                                                                                 Price = 0.5,
                                                                                 Name = "Chocolate"
                                                                             },
                                                                     [ProductKind.Tea] =
                                                                         new Product
                                                                             {
                                                                                 Kind = ProductKind.Tea,
                                                                                 Code = "T",
                                                                                 Price = 0.4,
                                                                                 Name = "Tea"
                                                                             },
                                                                     [ProductKind.OrangeJuice] =
                                                                         new Product
                                                                             {
                                                                                 Kind = ProductKind.OrangeJuice,
                                                                                 Code = "O",
                                                                                 Price = 0.6,
                                                                                 Name = "Orange",
                                                                                 IsCold = true
                                                                             }
                                                                 };

        private IProvideToday _todayProvider;

        public Logic(IProvideToday todayProvider)
        {
            _todayProvider = todayProvider;
        }

        public string Report()
        {
            return string.Join("\r\n ", this._orders.Select(x => ReportOfTheDay(x.Key, x.Value)));
        }

        public string Translate(Order order)
        {
            var product = _products[order.ProductKind];

            bool hasNotEnoughForDrink = order.Money < product.Price;
            if (hasNotEnoughForDrink) return $"M:Not enough money, missing {Math.Abs(order.Money - product.Price)}";

            var extraHotCode = !product.IsCold && order.ExtraHot ? "h" : string.Empty;

            var today = _todayProvider.GetToday();
            if (!_orders.ContainsKey(today)) this._orders.Add(today, new List<Order>());
            _orders[today].Add(order);

            int stick = order.Sugar >= 1 ? 1 : 0;
            return $"{product.Code}{extraHotCode}:{order.Sugar}:{stick}";
        }

        private string ReportOfTheDay(DateTime date, List<Order> ordersOftheDay)
        {
            var groupByProductType = ordersOftheDay.GroupBy(x => this._products[x.ProductKind].Name);

            return $"({date:d})| {ordersOftheDay.Sum(x => x.Money).ToString(CultureInfo.InvariantCulture)} euro, " + string.Empty
                                                                                                                   + string
                                                                                                                       .Join(
                                                                                                                           ", ",
                                                                                                                           groupByProductType
                                                                                                                               .Select(
                                                                                                                                   x =>
                                                                                                                                       x.Key
                                                                                                                                       + $": {x.Count()}"));
        }
    }
}