namespace CoffeeMachine
{
    using System;

    using NFluent;

    using NSubstitute;

    using NUnit.Framework;

    public class LogicShould
    {
        private Logic _logic;

        [SetUp]
        public void Setup()
        {
            _todayProvider = Substitute.For<IProvideToday>();
            _defaultToday = new DateTime(2018, 10, 19);
            _todayProvider.GetToday().Returns(_defaultToday);
            _logic = new Logic(_todayProvider);
        }

        [TestCase(BeverageKind.Coffee, "C:0:0")]
        [TestCase(BeverageKind.Chocolate, "H:0:0")]
        [TestCase(BeverageKind.OrangeJuice, "O:0:0")]
        [TestCase(BeverageKind.Tea, "T:0:0")]
        public void Return_1_drink_0_sugar_0_stick_When_customer_ask_for_a_specific_drink(BeverageKind beverageKind, string expected)
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = beverageKind,
                        Sugar = 0
                    });
            Check.That(actual).IsEqualTo(expected);
        }

        [TestCase(BeverageKind.Coffee, "C:1:1")]
        [TestCase(BeverageKind.Chocolate, "H:1:1")]
        [TestCase(BeverageKind.OrangeJuice, "O:1:1")]
        [TestCase(BeverageKind.Tea, "T:1:1")]
        public void Return_1_drink_1_sugar_1_stick_When_customer_ask_for_specific_drink(BeverageKind beverageKind, string expected)
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = beverageKind,
                        Sugar = 1
                    });
            Check.That(actual).IsEqualTo(expected);
        }

        [TestCase(BeverageKind.Coffee, "C:2:1")]
        [TestCase(BeverageKind.Chocolate, "H:2:1")]
        [TestCase(BeverageKind.OrangeJuice, "O:2:1")]
        [TestCase(BeverageKind.Tea, "T:2:1")]
        public void Return_1_drink_2_sugar_1_stick_When_customer_ask_for_a_specific_drink(BeverageKind beverageKind, string expected)
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = beverageKind,
                        Sugar = 2
                    });
            Check.That(actual).IsEqualTo(expected);
        }

        [Test]
        public void Return_1_coffee_0_sugar_0_stick_When_customer_ask_for_coffee_with_60_centimes()
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = BeverageKind.Coffee,
                        Sugar = 0,
                        Money = 0.6
                    });
            Check.That(actual).IsEqualTo("C:0:0");
        }

        /// <summary>
        /// Tea: 0.4; Coffee: 0.6; Chocolate: 0.5
        /// </summary>
        /// <param name="beverageKind"></param>
        /// <param name="money"></param>
        /// <param name="expected"></param>
        [TestCase(BeverageKind.Coffee, 0.6, "C:0:0")]
        [TestCase(BeverageKind.Chocolate, 0.5, "H:0:0")]
        [TestCase(BeverageKind.OrangeJuice, 0.6, "O:0:0")]
        [TestCase(BeverageKind.Tea, 0.4, "T:0:0")]
        public void Return_1_drink_0_sugar_0_stick_When_customer_ask_for_drink_with_enough_money(BeverageKind beverageKind, double money, string expected)
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = beverageKind,
                        Sugar = 0,
                        Money = money
                    });
            Check.That(actual).IsEqualTo(expected);
        }

        [TestCase(BeverageKind.Coffee, 0.5)]
        [TestCase(BeverageKind.Chocolate, 0.4)]
        [TestCase(BeverageKind.OrangeJuice, 0.5)]
        [TestCase(BeverageKind.Tea, 0.3)]
        public void Return_message_When_10_cents_is_not_enough_money_for_a_coffee(BeverageKind beverageKind, double missingMoney)
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = beverageKind,
                        Sugar = 0,
                        Money = 0.1
                    });
            Check.That(actual).IsEqualTo($"M:Not enough money, missing {missingMoney}");
        }

        [TestCase(BeverageKind.Coffee, "Ch:0:0")]
        [TestCase(BeverageKind.Chocolate, "Hh:0:0")]
        [TestCase(BeverageKind.Tea, "Th:0:0")]
        [TestCase(BeverageKind.OrangeJuice, "O:0:0")]
        public void Return_1_extra_hot_drink_0_sugar_0_stick_When_customer_ask_for_a_extra_hot_drink(BeverageKind beverageKind, string expected)
        {
            var actual = _logic.Translate(
                new Order
                    {
                        BeverageKind = beverageKind,
                        Sugar = 0,
                        ExtraHot = true
                    });
            Check.That(actual).IsEqualTo(expected);
        }

        [Test]
        public void Report_what_is_sold_and_when_orders_are_one_coffe_with_one_sugar_and_an_orange_juice()
        {
            _logic.Translate(new Order(BeverageKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            _logic.Translate(new Order(BeverageKind.OrangeJuice, extraHot: false, money: 0.6, sugar: 1));

            string report = _logic.Report();

            Check.That(report).IsEqualTo($"({_defaultToday:d})| 1.2 euro, Coffee: 1, Orange: 1");
        }

        [Test]
        public void Report_what_is_sold_and_when_orders_are_2_coffe_with_one_sugar_and_an_orange_juice()
        {
            _logic.Translate(new Order(BeverageKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            _logic.Translate(new Order(BeverageKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            _logic.Translate(new Order(BeverageKind.OrangeJuice, extraHot: false, money: 0.6, sugar: 1));

            string report = _logic.Report();

            Check.That(report).IsEqualTo($"({_defaultToday:d})| 1.8 euro, Coffee: 2, Orange: 1");
        }

        [Test]
        [Repeat(500)]
        public void Report_what_is_sold_and_when_orders_are_2_coffe_with_one_sugar_and_an_orange_juice_two_orders_in_different_days()
        {
            _logic.Translate(new Order(BeverageKind.Coffee, extraHot: false, money: 0.6, sugar: 1));

            _todayProvider.GetToday().Returns(DateTime.Today.AddDays(1));

            _logic.Translate(new Order(BeverageKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            _logic.Translate(new Order(BeverageKind.OrangeJuice, extraHot: false, money: 0.6, sugar: 1));

            string report = _logic.Report();

            Check.That(report).IsEqualTo($"({_defaultToday:d})| 0.6 euro, Coffee: 1\r\n ({DateTime.Today.AddDays(1):d})| 1.2 euro, Coffee: 1, Orange: 1");
        }

        protected internal IProvideToday _todayProvider;

        protected internal DateTime _defaultToday;
    }
}