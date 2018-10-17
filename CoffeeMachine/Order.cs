namespace CoffeeMachine
{
    public class Order
    {
        public ProductKind ProductKind { get; set; }

        public int Sugar { get; set; }
        public double Money { get; set; } = 0.6;

        public bool ExtraHot { get; set; } = false;

        public Order()
        {
            
        }

        public Order(ProductKind productKind, bool extraHot, double money, int sugar)
        {
            this.ProductKind = productKind;
            this.ExtraHot = extraHot;
            this.Money = money;
            this.Sugar = sugar;
        }
    }
}