using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorTests : IDisposable
{
    private readonly FakeCoffeeCountStore _coffeeCountStore;
    private readonly MachineDataProcessor _machineDataProcessor;

    public MachineDataProcessorTests()
    {
        _coffeeCountStore = new FakeCoffeeCountStore();
        _machineDataProcessor = new MachineDataProcessor(_coffeeCountStore);
    }

    [Fact]
    public void ShouldSaveCountPerCoffeeType()
    {
        MachineDataItem[] items = [
            new("Cappuccino", new DateTime(2022,10,27,8,0,0,DateTimeKind.Unspecified)),
            new("Cappuccino", new DateTime(2022,10,27,9,0,0,DateTimeKind.Unspecified)),
            new("Espresso", new DateTime(2022,10,27,10,0,0,DateTimeKind.Unspecified))
        ];

        _machineDataProcessor.ProcessItems(items);

        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    [Fact]
    public void ShouldClearPreviousCoffeeCount()
    {
        MachineDataItem[] items = [
            new("Cappuccino", new DateTime(2022,10,27,8,0,0,DateTimeKind.Unspecified)),
        ];

        _machineDataProcessor.ProcessItems(items);
        _machineDataProcessor.ProcessItems(items);

        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);
        foreach (var item in _coffeeCountStore.SavedItems)
        {
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        MachineDataItem[] items = [
            new("Cappuccino", new DateTime(2022,10,27,8,0,0,DateTimeKind.Unspecified)),
            new("Cappuccino", new DateTime(2022,10,27,7,0,0,DateTimeKind.Unspecified)),
            new("Cappuccino", new DateTime(2022,10,27,7,10,0,DateTimeKind.Unspecified)),
            new("Cappuccino", new DateTime(2022,10,27,9,0,0,DateTimeKind.Unspecified)),
            new("Espresso", new DateTime(2022,10,27,10,0,0,DateTimeKind.Unspecified)),
            new("Espresso", new DateTime(2022,10,27,10,0,0,DateTimeKind.Unspecified))
        ];

        _machineDataProcessor.ProcessItems(items);

        Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

        var item = _coffeeCountStore.SavedItems[0];
        Assert.Equal("Cappuccino", item.CoffeeType);
        Assert.Equal(2, item.Count);

        item = _coffeeCountStore.SavedItems[1];
        Assert.Equal("Espresso", item.CoffeeType);
        Assert.Equal(1, item.Count);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _coffeeCountStore.SavedItems.Clear();
    }
}

public class FakeCoffeeCountStore : ICoffeeCountStore
{
    public List<CoffeeCountItem> SavedItems { get; } = new();

    public void Save(CoffeeCountItem coffeeCount)
    {
        SavedItems.Add(coffeeCount);
    }
}