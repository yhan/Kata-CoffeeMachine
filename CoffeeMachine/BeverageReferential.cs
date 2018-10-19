namespace CoffeeMachine
{
    using System.Collections.Generic;

    public class BeverageReferential
    {
        private readonly Dictionary<BeverageKind, Product> _beverages = new Dictionary<BeverageKind, Product>
                                                                            {
                                                                                [BeverageKind.Coffee] = new Product
                                                                                                            {
                                                                                                                Kind = BeverageKind.Coffee,
                                                                                                                Code = "C",
                                                                                                                Price = 0.6,
                                                                                                                Name = "Coffee"
                                                                                                            },
                                                                                [BeverageKind.Chocolate] = new Product
                                                                                                               {
                                                                                                                   Kind = BeverageKind.Chocolate,
                                                                                                                   Code = "H",
                                                                                                                   Price = 0.5,
                                                                                                                   Name = "Chocolate"
                                                                                                               },
                                                                                [BeverageKind.Tea] = new Product
                                                                                                         {
                                                                                                             Kind = BeverageKind.Tea,
                                                                                                             Code = "T",
                                                                                                             Price = 0.4,
                                                                                                             Name = "Tea"
                                                                                                         },
                                                                                [BeverageKind.OrangeJuice] = new Product
                                                                                                                 {
                                                                                                                     Kind = BeverageKind.OrangeJuice,
                                                                                                                     Code = "O",
                                                                                                                     Price = 0.6,
                                                                                                                     Name = "Orange",
                                                                                                                     IsCold = true
                                                                                                                 }
                                                                            };

        public Product GetBeverage(BeverageKind _beverageKind)
        {
            return _beverages[_beverageKind];
        }
    }
}