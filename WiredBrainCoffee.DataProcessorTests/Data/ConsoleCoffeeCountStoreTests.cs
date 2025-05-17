using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data;

public class ConsoleCoffeeCountStoreTests
{
    [Fact]
    public void ShouldWriteOutputToConsole()
    {
        var item = new CoffeeCountItem("Cappuccino", 5);
        var stringWriter = new StringWriter();
        var consoleCoffeeCountStore = new ConsoleCoffeeCountStore(stringWriter);

        consoleCoffeeCountStore.Save(item);

        var result = stringWriter.ToString();
        Assert.Equal($"{item.CoffeeType}:{item.Count}{Environment.NewLine}", result);
    }
}