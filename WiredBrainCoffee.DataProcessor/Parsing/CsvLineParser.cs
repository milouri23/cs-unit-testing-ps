using System;
using System.Collections.Generic;
using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing
{
    public static class CsvLineParser
    {
        public static MachineDataItem[] Parse(string[] csvlines)
        {
            var machineDataItems = new List<MachineDataItem>();

            foreach (var line in csvlines)
            {
                var machineDataItem = Parse(line);

                machineDataItems.Add(machineDataItem);
            }

            return [.. machineDataItems];
        }

        private static MachineDataItem Parse(string csvLine)
        {
            var lineItems = csvLine.Split(';');

            return new MachineDataItem(
                coffeeType: lineItems[0],
                createdAt: DateTime.Parse(lineItems[1], CultureInfo.InvariantCulture)
            );
        }
    }
}