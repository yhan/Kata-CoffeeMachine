namespace CoffeeMachine
{
    public class Order
    {
        public ProductKind ProductKind { get; set; }

        public int Sugar { get; set; }
        public double Money { get; set; } = 0.6;

        public bool ExtraHot { get; set; } = false;
    }
}