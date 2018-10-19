namespace CoffeeMachine
{
    public class Order
    {
        public Order()
        {
        }

        public Order(BeverageKind beverageKind, bool extraHot, double money, int sugar)
        {
            BeverageKind = beverageKind;
            ExtraHot = extraHot;
            Money = money;
            Sugar = sugar;
        }

        public BeverageKind BeverageKind { get; set; }

        public int Sugar { get; set; }

        public double Money { get; set; } = 0.6;

        public bool ExtraHot { get; set; }
    }
}