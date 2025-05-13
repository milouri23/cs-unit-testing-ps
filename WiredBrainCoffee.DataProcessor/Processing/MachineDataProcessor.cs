using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessor(int initialCapacity = 4)
    {
        private readonly Dictionary<string, int> _countPerCoffeeType = new(initialCapacity);

        public void ProcessItems(MachineDataItem[] dataItems)
        {
            _countPerCoffeeType.Clear();

            foreach (var dataItem in dataItems)
            {
                ProcessItem(dataItem);
            }

            SaveCountPerCoffeeType();
        }

        private void ProcessItem(MachineDataItem dataItem)
        {
            // Obtiene una referencia al valor. Si la clave no existe,
            // la añade con el valor por defecto para int (0) y devuelve una referencia a ese 0.
            ref int countRef = ref CollectionsMarshal.GetValueRefOrAddDefault(_countPerCoffeeType, dataItem.CoffeeType, out bool _);

            // Incrementa el valor directamente a través de la referencia.
            // Si no existía (exists == false), 0 se convierte en 1.
            // Si existía, su valor actual se incrementa.
            countRef++;
        }

        private void SaveCountPerCoffeeType()
        {
            var stringBuilder = new StringBuilder();

            foreach (var entry in _countPerCoffeeType)
            {
                stringBuilder.Append(entry.Key);
                stringBuilder.Append(':');
                stringBuilder.Append(entry.Value);
                stringBuilder.AppendLine();
            }

            Console.WriteLine(stringBuilder);
        }
    }
}