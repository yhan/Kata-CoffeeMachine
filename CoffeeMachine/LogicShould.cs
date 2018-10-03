namespace CoffeeMachine
{
    using NFluent;

    using NUnit.Framework;

    public class LogicShould
    {
        [Test]
        public void Return_1_coffee_0_sugar_0_stick_When_customer_ask_for_coffee()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Coffee, Sugar = 0 });
            Check.That(actual).IsEqualTo("C:0:0");
        }

        [Test]
        public void Return_1_coffee_1_sugar_1_stick_When_customer_ask_for_coffee()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Coffee, Sugar = 1 });
            Check.That(actual).IsEqualTo("C:1:1");
        }

        [Test]
        public void Return_1_coffee_2_sugar_1_stick_When_customer_ask_for_coffee()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Coffee, Sugar = 2 });
            Check.That(actual).IsEqualTo("C:2:1");
        }

        [Test]
        public void Return_1_tea_0_sugar_0_stick_When_customer_ask_for_a_tea()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Tea, Sugar = 0 });
            Check.That(actual).IsEqualTo("T:0:0");
        }

        [Test]
        public void Return_1_tea_1_sugar_1_stick_When_customer_ask_for_tea()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Tea, Sugar = 1 });
            Check.That(actual).IsEqualTo("T:1:1");
        }

        [Test]
        public void Return_1_tea_2_sugar_1_stick_When_customer_ask_for_tea()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Tea, Sugar = 2 });
            Check.That(actual).IsEqualTo("T:2:1");
        }

        [Test]
        public void Return_1_chocolate_0_sugar_0_stick_When_customer_ask_for_a_chocolate()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Chocolate, Sugar = 0 });
            Check.That(actual).IsEqualTo("H:0:0");
        }


        [Test]
        public void Return_1_chocolate_1_sugar_1_stick_When_customer_ask_for_chocolate()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Chocolate, Sugar = 1 });
            Check.That(actual).IsEqualTo("H:1:1");
        }

        [Test]
        public void Return_1_chocolate_2_sugar_1_stick_When_customer_ask_for_chocolate()
        {
            var logic = new Logic();
            var actual = logic.Translate(new Order { Product = Product.Chocolate, Sugar = 2 });
            Check.That(actual).IsEqualTo("H:2:1");
        }
    }
}