using System;
using System.Collections.Generic;
using System.Linq;
using GameEventSystem;
using Player.ResourcesUsable;
using ScriptableObjects;
using UnityEngine;

namespace Player {
public class Inventory : MonoBehaviour {
    private readonly List<InventorySlot> _items = new();
    public List<InventorySlot> Items => _items;

    public event EventHandler<InventorySlotEventArgs> InventoryChanged;
    [SerializeField] private GameEvent showChoiceDialog;

    [SerializeField] private List<ResourceCharacteristics> allResourceTypes;

    private void Awake() {
        foreach (var resourceType in allResourceTypes) {
            var item = new InventorySlot(resourceType, 0, 0);
            _items.Add(item);
        }
    }

    /**
     * Set returning to true if it comes back to the inventory after being called to duty.
     * Else it comes from the reward dialog (so put returning to false)
     */
    public void addItem(ResourceCharacteristics characteristics, int number,
        bool returning = false) {
        var indexSlot = _items.FindIndex(slot => slot.Characteristics == characteristics);
        var inventorySlot = _items[indexSlot];
        inventorySlot.NumberAvailable += number;
        if (returning) {
            InventoryChanged?.Invoke(this, new InventorySlotEventArgs(inventorySlot, indexSlot));
            return;
        }

        // newly added resources
        inventorySlot.NumberTotal += number;
        InventoryChanged?.Invoke(this, new InventorySlotEventArgs(inventorySlot, indexSlot));
        showChoiceDialog.sentBool = false;
        showChoiceDialog.Raise();
    }

    public void testAddItem() {
        var resource = allResourceTypes.First();
        addItem(resource, 3);
    }

    public void callToMap(ResourceCharacteristics characteristics, Transform position) {
        var indexSlot = _items.FindIndex(slot => slot.Characteristics == characteristics);
        var item = _items[indexSlot];
        if (item.NumberAvailable <= 0) return;
        item.NumberAvailable--;
        var newResource = Instantiate(characteristics.prefab, position);
        InventoryChanged?.Invoke(this, new InventorySlotEventArgs(item, indexSlot));
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

public class InventorySlotEventArgs : EventArgs {
    public InventorySlotEventArgs(InventorySlot inventorySlot, int index) {
        this.inventorySlot = inventorySlot;
        this.index = index;
    }

    public readonly InventorySlot inventorySlot;
    public readonly int index;
}
}