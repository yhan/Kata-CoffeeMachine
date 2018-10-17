using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine
{
    public class Product
    {
        public ProductKind Kind { get; set; }

        public string Code { get; set; }

        public double Price { get; set; }

        public bool IsCold { get; set; }

        public string Name { get; set; }
    }
}
