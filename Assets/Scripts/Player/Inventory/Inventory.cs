using System;
using System.Collections.Generic;
using System.Linq;
using Environment;
using GameEventSystem;
using Player.ResourcesUsable;
using ScriptableObjects;
using Shop;
using UnityEngine;

namespace Player.Inventory {
public class Inventory : MonoBehaviour {
    private readonly List<InventorySlot> _items = new();
    public List<InventorySlot> Items => _items;

    public event EventHandler<InventorySlotEventArgs> InventoryChanged;
    [SerializeField] private GameEvent showChoiceDialog;
    [SerializeField] private GameEvent showResourceLostDialog;

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
    private void addItem(ResourceCharacteristics characteristics, int number,
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

    private void onResourceLost(object sender, InventoryEventArgs resourceType) {
        var indexSlot = _items.FindIndex(slot => slot.Characteristics == resourceType.item);
        var inventorySlot = _items[indexSlot];
        inventorySlot.NumberTotal--;
        InventoryChanged?.Invoke(this, new InventorySlotEventArgs(inventorySlot, indexSlot));
        showResourceLostDialog.sentString = resourceType.item.name;
        showResourceLostDialog.Raise();
    }

    public void choosePack(ShopChoice choice) {
        addItem(choice.Characteristics, choice.Number);
    }

    public void testAddItem() {
        foreach (var type in allResourceTypes) {
            addItem(type, 3);
        }
    }
    
    public void fillWithStarter() {
        foreach (var starters in allResourceTypes.Where(characteristics => characteristics.isStarter)) {
            addItem(starters, 1);
        }
    }

    public void callToMap(ResourceCharacteristics characteristics, TileGraphic tile) {
        var indexSlot = _items.FindIndex(slot => slot.Characteristics == characteristics);
        var item = _items[indexSlot];
        if (item.NumberAvailable <= 0) return;
        item.NumberAvailable--;
        var newResource = Instantiate(characteristics.prefab, tile.transform);
        InventoryChanged?.Invoke(this, new InventorySlotEventArgs(item, indexSlot));
        var resource = newResource.GetComponent<Resource>();
        resource.Tile = tile;
        resource.ReturnToInventory += onReturnToInventory;
        resource.OnLost += onResourceLost;
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