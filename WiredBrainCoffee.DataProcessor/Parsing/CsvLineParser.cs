using System;
using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing
{
    public static class CsvLineParser
    {
        public static MachineDataItem[] Parse(string[] csvlines)
        {
            var machineDataItems = new MachineDataItem[csvlines.Length];

            for (int i = 0; i < csvlines.Length; i++)
            {
                var line = csvlines[i];
                machineDataItems[i] = Parse(line);
            }

            return machineDataItems;
        }

        private static MachineDataItem Parse(ReadOnlySpan<char> csvLine)
        {
            int separatorIndex = csvLine.IndexOf(';');

            ReadOnlySpan<char> coffeeTypeSpan = csvLine[..separatorIndex];
            ReadOnlySpan<char> createdAtSpan = csvLine[(separatorIndex + 1)..];

            return new MachineDataItem(
                coffeeType: coffeeTypeSpan.ToString(),
                createdAt: DateTime.Parse(createdAtSpan, CultureInfo.InvariantCulture)
            );
        }
    }
}