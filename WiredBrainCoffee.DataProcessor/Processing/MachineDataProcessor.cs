using System.Runtime.InteropServices;
using System.Text;
using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessor(ICoffeeCountStore coffeeCountStore, int initialCapacity = 4)
{
    private readonly Dictionary<string, int> _countPerCoffeeType = new Dictionary<string, int>(initialCapacity);
    private ICoffeeCountStore _coffeeCountStore = coffeeCountStore;

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
        foreach (var entry in _countPerCoffeeType)
        {
            _coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
        }
    }
}