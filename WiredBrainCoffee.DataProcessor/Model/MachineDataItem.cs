using System;

namespace WiredBrainCoffee.DataProcessor.Model
{
    public class MachineDataItem
    {
        public string CoffeeType { get; set; }
        public DateTime CreatedAt { get; set; }

        public MachineDataItem(string coffeeType, DateTime createdAt)
        {
            CoffeeType = coffeeType;
            CreatedAt = createdAt;
        }
    }
}