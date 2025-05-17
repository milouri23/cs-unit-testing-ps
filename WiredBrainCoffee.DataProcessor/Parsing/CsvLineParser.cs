using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing;

public static class CsvLineParser
{
    public static MachineDataItem[] Parse(string[] csvlines)
    {
        var machineDataItems = new MachineDataItem[csvlines.Length];

        int validItemsCount = 0;

        for (int i = 0; i < csvlines.Length; i++)
        {
            var line = csvlines[i];

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            machineDataItems[validItemsCount] = Parse(line);
            validItemsCount++;
        }

        if (validItemsCount == 0)
        {
            return Array.Empty<MachineDataItem>();
        }

        if (validItemsCount < machineDataItems.Length)
        {
            Array.Resize(ref machineDataItems, validItemsCount);
        }

        return machineDataItems;
    }

    private static MachineDataItem Parse(ReadOnlySpan<char> csvLine)
    {
        int separatorIndex = csvLine.IndexOf(';');

        if (separatorIndex == -1)
        {
            throw new Exception($"Invalid csv line: Missing separator in: {csvLine}");
        }

        ReadOnlySpan<char> createdAtSpan = csvLine[(separatorIndex + 1)..].Trim();

        if (createdAtSpan.IndexOf(';') != -1)
        {
            throw new Exception($"Invalid csv line: Multiple separator in: {csvLine}");
        }

        ReadOnlySpan<char> coffeeTypeSpan = csvLine[..separatorIndex].Trim();

        if (!DateTime.TryParse(createdAtSpan, CultureInfo.InvariantCulture, out DateTime createdAt))
        {
            throw new Exception($"Invalid datetime in csv line: {csvLine}");
        }

        return new MachineDataItem(
            CoffeeType: coffeeTypeSpan.ToString(),
            CreatedAt: createdAt
        );
    }
}