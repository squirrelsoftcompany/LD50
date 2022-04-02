using ScriptableObjects;

namespace Player {
public class InventorySlot {
    public ResourceCharacteristics Characteristics { get; set; }
    public int NumberAvailable { get; set; }
    public int NumberTotal { get; set; }

    public InventorySlot(ResourceCharacteristics characteristics, int numberAvailable,
        int numberTotal) {
        Characteristics = characteristics;
        NumberAvailable = numberAvailable;
        NumberTotal = numberTotal;
    }
}
}