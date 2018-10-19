namespace CoffeeMachine
{
    public class Product
    {
        public BeverageKind Kind { get; set; }

        public string Code { get; set; }

        public double Price { get; set; }

        public bool IsCold { get; set; }

        public string Name { get; set; }
    }
}