namespace CoffeeMachine
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class Logic
    {
        private readonly BeverageReferential _beverageReferential = new BeverageReferential();

        private readonly Dictionary<DateTime, List<Order>> _orders = new Dictionary<DateTime, List<Order>>();

        private readonly IProvideToday _todayProvider;
        private readonly IBeverageQuantityChecker _beverageQuantityChecker;
        private readonly IEmailNotifier _emailNotifier;

        public Logic(IProvideToday todayProvider, IBeverageQuantityChecker beverageQuantityChecker, IEmailNotifier emailNotifier)
        {
            _todayProvider = todayProvider;
            _beverageQuantityChecker = beverageQuantityChecker;
            _emailNotifier = emailNotifier;
        }

        public string Translate(Order order)
        {
            var beverage = _beverageReferential.GetBeverage(order.BeverageKind);

            if (_beverageQuantityChecker.IsEmpty(beverage.Code))
            {
                _emailNotifier.NotifyMissingDrink(beverage.Code);
                return $"M:Shortage for {beverage.Name}";
            }

            if (ReturnNotEnoughMoneyWhenNecessary(order, beverage, out var message))
            {
                return message;
            }

            var extraHotCode = !beverage.IsCold && order.ExtraHot ? "h" : string.Empty;

            TakeOrder(order);

            return BuildMessage(order, beverage, extraHotCode);
        }

        public string Report()
        {
            return string.Join("\r\n ", _orders.Select(x => ReportOfTheDay(x.Key, x.Value)));
        }

        private string ReportOfTheDay(DateTime date, IReadOnlyCollection<Order> ordersOfTheDay)
        {
            var turnover = CalculateTurnover(ordersOfTheDay);
            var beveragesSummary = BuildBeveragesSummary(ordersOfTheDay);

            return $"({date:d})| {turnover}, {beveragesSummary}";
        }

        private static string BuildMessage(Order order, Product product, string extraHotCode)
        {
            var stick = order.Sugar >= 1 ? 1 : 0;
            return $"{product.Code}{extraHotCode}:{order.Sugar}:{stick}";
        }

        private void TakeOrder(Order order)
        {
            var today = _todayProvider.GetToday();
            if (!_orders.ContainsKey(today))
            {
                _orders.Add(today, new List<Order>());
            }

            _orders[today].Add(order);
        }

        private static bool ReturnNotEnoughMoneyWhenNecessary(Order order, Product product, out string message)
        {
            message = string.Empty;
            bool hasNotEnoughForDrink = order.Money < product.Price;
            if (hasNotEnoughForDrink)
            {
                message = $"M:Not enough money, missing {Math.Abs(order.Money - product.Price)}";
                return true;
            }

            return false;
        }

        private string BuildBeveragesSummary(IEnumerable<Order> ordersOfTheDay)
        {
            var groupByProductType = ordersOfTheDay.GroupBy(
                x =>
                    {
                        var beverageName = _beverageReferential.GetBeverage(x.BeverageKind).Name;
                        return beverageName;
                    });
            var beveragesSummary = string.Join(
                ", ",
                groupByProductType.Select(
                    x =>
                        {
                            var beverageNameAndQuantity = $"{x.Key}: {x.Count()}";
                            return beverageNameAndQuantity;
                        }));
            return beveragesSummary;
        }

        private static string CalculateTurnover(IEnumerable<Order> ordersOfTheDay)
        {
            return $"{ordersOfTheDay.Sum(x => x.Money).ToString(CultureInfo.InvariantCulture)} euro";
        }
    }
}