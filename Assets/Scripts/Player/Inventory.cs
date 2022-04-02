using System;
using System.Collections.Generic;
using System.Linq;
using GameEventSystem;
using Player.ResourcesUsable;
using ScriptableObjects;
using UnityEngine;

namespace Player {
public class Inventory : MonoBehaviour {
    private List<InventorySlot> _items = new();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    [SerializeField] private GameEvent showChoiceDialog;

    [SerializeField] private List<ResourceCharacteristics> allResourceTypes;

    // Start is called before the first frame update
    private void Start() {
        foreach (var resourceType in allResourceTypes) {
            _items.Add(new InventorySlot(resourceType, 0, 0));
        }
    }

    /**
     * Set returning to true if it comes back to the inventory after being called to duty.
     * Else it comes from the reward dialog (so put returning to false)
     */
    public void addItem(ResourceCharacteristics characteristics, int number,
        bool returning = false) {
        var inventorySlot = _items.First(slot => slot.Characteristics == characteristics);
        inventorySlot.NumberAvailable += number;
        ItemAdded?.Invoke(this, new InventoryEventArgs(characteristics));
        if (returning) return;
        // newly added resources
        inventorySlot.NumberTotal += number;
        showChoiceDialog.sentBool = false;
        showChoiceDialog.Raise();
    }

    public void testAddItem() {
        var resource = allResourceTypes.First();
        var instantiate = Instantiate(resource.prefab, transform);
        var inventoryItem = instantiate.GetComponent<Resource>();

        addItem(inventoryItem.Characteristics, 3);
    }

    public void callToMap(ResourceCharacteristics resourceType, Transform position) {
        var newResource = Instantiate(resourceType.prefab, transform);
        var resource = newResource.GetComponent<Resource>();
        resource.ReturnToInventory += onReturnToInventory;
    }

    private void onReturnToInventory(object sender, InventoryEventArgs resourceType) {
        addItem(resourceType.item, resourceType.number, true);
    }
}

public class InventoryEventArgs : EventArgs {
    public InventoryEventArgs(ResourceCharacteristics item, int number = 1) {
        this.item = item;
        this.number = number;
    }

    public ResourceCharacteristics item;
    public int number;
}
}