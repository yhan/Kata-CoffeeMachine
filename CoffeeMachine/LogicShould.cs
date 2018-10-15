namespace CoffeeMachine
{
    using NFluent;
    using NUnit.Framework;

    public class LogicShould
    {
        private Logic _logic;

        [SetUp]
        public void Setup()
        {
            _logic = new Logic();
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

    }
}