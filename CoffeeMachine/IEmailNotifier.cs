using System;

namespace CoffeeMachine
{
    public interface IEmailNotifier
    {
        void NotifyMissingDrink(String drink);
    }
}