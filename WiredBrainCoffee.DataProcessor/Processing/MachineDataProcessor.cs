using System.Runtime.InteropServices;
using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessor(ICoffeeCountStore coffeeCountStore, int initialCapacity = 4)
{
    private readonly Dictionary<string, int> _countPerCoffeeType = new Dictionary<string, int>(initialCapacity);
    private readonly ICoffeeCountStore _coffeeCountStore = coffeeCountStore;
    private MachineDataItem? _previousItem;

    public void ProcessItems(MachineDataItem[] dataItems)
    {
        ClearPreviousProcessing();

        foreach (var dataItem in dataItems)
        {
            ProcessItem(dataItem);
        }

        SaveCountPerCoffeeType();
    }

    private void ClearPreviousProcessing()
    {
        _previousItem = null;
        _countPerCoffeeType.Clear();
    }

    private void ProcessItem(MachineDataItem dataItem)
    {
        if (!IsNewerThanPreviousItem(dataItem))
        {
            return;
        }

        // Obtiene una referencia al valor. Si la clave no existe,
        // la añade con el valor por defecto para int (0) y devuelve una referencia a ese 0.
        ref int countRef = ref CollectionsMarshal.GetValueRefOrAddDefault(_countPerCoffeeType, dataItem.CoffeeType, out bool _);

        // Incrementa el valor directamente a través de la referencia.
        // Si no existía (exists == false), 0 se convierte en 1.
        // Si existía, su valor actual se incrementa.
        countRef++;

        _previousItem = dataItem;
    }

    private bool IsNewerThanPreviousItem(MachineDataItem dataItem)
    {
        return _previousItem == null
            || _previousItem.CreatedAt < dataItem.CreatedAt;
    }

    private void SaveCountPerCoffeeType()
    {
        foreach (var entry in _countPerCoffeeType)
        {
            _coffeeCountStore.Save(new CoffeeCountItem(entry.Key, entry.Value));
        }
    }
}