using System.Text;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data;

public class ConsoleCoffeeCountStore(TextWriter textWriter) : ICoffeeCountStore
{
    private readonly TextWriter _textWriter = textWriter;

    public ConsoleCoffeeCountStore() : this(Console.Out)
    {
    }

    public void Save(CoffeeCountItem coffeeCount)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(coffeeCount.CoffeeType);
        stringBuilder.Append(':');
        stringBuilder.Append(coffeeCount.Count);

        _textWriter.WriteLine(stringBuilder);
    }
}