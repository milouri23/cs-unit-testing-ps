using System;

namespace WiredBrainCoffee.DataProcessor.Model
{
    public class MachineDataItem(string coffeeType, DateTime createdAt)
    {
        public string CoffeeType { get; } = coffeeType;
        public DateTime CreatedAt { get; } = createdAt;
    }
}