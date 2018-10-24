using System;

namespace CoffeeMachine
{
    public interface IBeverageQuantityChecker
    {
        bool IsEmpty(String drink);
    }
}