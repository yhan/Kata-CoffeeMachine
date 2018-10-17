namespace CoffeeMachine
{
    using System;

    using NFluent;

    using NSubstitute;

    using NUnit.Framework;

    public class LogicShould
    {
        private Logic _logic;

        protected internal IProvideToday _todayProvider;

        [SetUp]
        public void Setup()
        {
            this._todayProvider = Substitute.For<IProvideToday>();
            this._todayProvider.GetToday().Returns(DateTime.Today);
            _logic = new Logic(this._todayProvider);
        }


        [TestCase(ProductKind.Coffee,"C:0:0")]
        [TestCase(ProductKind.Chocolate,  "H:0:0")]
        [TestCase(ProductKind.OrangeJuice, "O:0:0")]
        [TestCase(ProductKind.Tea, "T:0:0")]
        public void Return_1_drink_0_sugar_0_stick_When_customer_ask_for_a_specific_drink(ProductKind productKind, string expected)
        {
            var actual = _logic.Translate(new Order {ProductKind = productKind, Sugar = 0});
            Check.That(actual).IsEqualTo(expected);
        }

        [TestCase(ProductKind.Coffee, "C:1:1")]
        [TestCase(ProductKind.Chocolate, "H:1:1")]
        [TestCase(ProductKind.OrangeJuice, "O:1:1")]
        [TestCase(ProductKind.Tea, "T:1:1")]
        public void Return_1_drink_1_sugar_1_stick_When_customer_ask_for_specific_drink(ProductKind productKind, string expected)
        {
            var actual = _logic.Translate(new Order {ProductKind = productKind, Sugar = 1});
            Check.That(actual).IsEqualTo(expected);
        }

        [TestCase(ProductKind.Coffee, "C:2:1")]
        [TestCase(ProductKind.Chocolate, "H:2:1")]
        [TestCase(ProductKind.OrangeJuice, "O:2:1")]
        [TestCase(ProductKind.Tea, "T:2:1")]
        public void Return_1_drink_2_sugar_1_stick_When_customer_ask_for_a_specific_drink(ProductKind productKind, string expected)
        {
            var actual = _logic.Translate(new Order {ProductKind = productKind, Sugar = 2});
            Check.That(actual).IsEqualTo(expected);
        }
        
        [Test]
        public void Return_1_coffee_0_sugar_0_stick_When_customer_ask_for_coffee_with_60_centimes()
        {
            
            var actual = _logic.Translate(new Order {ProductKind = ProductKind.Coffee, Sugar = 0, Money = 0.6});
            Check.That(actual).IsEqualTo("C:0:0");
        }
        /// <summary>
        /// Tea: 0.4; Coffee: 0.6; Chocolate: 0.5
        /// </summary>
        /// <param name="productKind"></param>
        /// <param name="money"></param>
        /// <param name="expected"></param>
        [TestCase(ProductKind.Coffee, 0.6, "C:0:0")]
        [TestCase(ProductKind.Chocolate, 0.5, "H:0:0")]
        [TestCase(ProductKind.OrangeJuice, 0.6, "O:0:0")]
        [TestCase(ProductKind.Tea, 0.4, "T:0:0")]
        public void Return_1_drink_0_sugar_0_stick_When_customer_ask_for_drink_with_enough_money(ProductKind productKind, double money, string expected)
        {
            var actual = _logic.Translate(new Order { ProductKind = productKind, Sugar = 0, Money = money });
            Check.That(actual).IsEqualTo(expected);
        }

        [TestCase(ProductKind.Coffee, 0.5)]
        [TestCase(ProductKind.Chocolate, 0.4)]
        [TestCase(ProductKind.OrangeJuice, 0.5)]
        [TestCase(ProductKind.Tea, 0.3)]
        public void Return_message_When_10_cents_is_not_enough_money_for_a_coffee(ProductKind productKind, double missingMoney)
        {
            var actual = _logic.Translate(new Order {ProductKind = productKind, Sugar = 0, Money = 0.1});
            Check.That(actual).IsEqualTo($"M:Not enough money, missing {missingMoney}");
        }

        [TestCase(ProductKind.Coffee, "Ch:0:0")]
        [TestCase(ProductKind.Chocolate, "Hh:0:0")]
        [TestCase(ProductKind.Tea, "Th:0:0")]
        [TestCase(ProductKind.OrangeJuice, "O:0:0")]
        public void Return_1_extra_hot_drink_0_sugar_0_stick_When_customer_ask_for_a_extra_hot_drink(ProductKind productKind, string expected)
        {
            var actual = _logic.Translate(new Order { ProductKind = productKind, Sugar = 0, ExtraHot = true });
            Check.That(actual).IsEqualTo(expected);
        }


        [Test]
        public void Report_what_is_sold_and_when_orders_are_one_coffe_with_one_sugar_and_an_orange_juice()
        {
            this._logic.Translate(new Order(ProductKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            this._logic.Translate(new Order(ProductKind.OrangeJuice, extraHot: false, money: 0.6, sugar: 1));

            string report = this._logic.Report();

            Check.That(report).IsEqualTo("(17/10/2018)| 1.2 euro, Coffee: 1, Orange: 1");
        }

        [Test]
        public void Report_what_is_sold_and_when_orders_are_2_coffe_with_one_sugar_and_an_orange_juice()
        {
            this._logic.Translate(new Order(ProductKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            this._logic.Translate(new Order(ProductKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            this._logic.Translate(new Order(ProductKind.OrangeJuice, extraHot: false, money: 0.6, sugar: 1));

            string report = this._logic.Report();

            Check.That(report).IsEqualTo($"({DateTime.Today:d})| 1.8 euro, Coffee: 2, Orange: 1");
        }


        [Test]
        [Repeat(500)]
        public void Report_what_is_sold_and_when_orders_are_2_coffe_with_one_sugar_and_an_orange_juice_two_orders_in_different_days()
        {
            this._logic.Translate(new Order(ProductKind.Coffee, extraHot: false, money: 0.6, sugar: 1));

            _todayProvider.GetToday().Returns(DateTime.Today.AddDays(1));
            
            this._logic.Translate(new Order(ProductKind.Coffee, extraHot: false, money: 0.6, sugar: 1));
            this._logic.Translate(new Order(ProductKind.OrangeJuice, extraHot: false, money: 0.6, sugar: 1));

            string report = this._logic.Report();

            Check.That(report).IsEqualTo($"({DateTime.Today:d})| 0.6 euro, Coffee: 1\r\n ({DateTime.Today.AddDays(1):d})| 1.2 euro, Coffee: 1, Orange: 1");
        }
    }
}