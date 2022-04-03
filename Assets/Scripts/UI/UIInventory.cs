// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
public class UIInventory : MonoBehaviour {
    private Inventory _inventory;
    private List<GameObject> _uiSlots;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private int startX = -200;
    [SerializeField] private int intervalX = 100;
    [SerializeField] private int y = 50;

    private void Awake() {
        _inventory = FindObjectOfType<Inventory>();
    }

    private void Start() {
        _inventory.InventoryChanged += showNewItem;
        _uiSlots = new List<GameObject>();
        for (var i = 0; i < _inventory.Items.Count; i++) {
            var inventorySlot = _inventory.Items[i];
            var ui = Instantiate(inventoryUI, transform);
            ui.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(startX + i * intervalX, y);
            changeImageAndText(ui, inventorySlot);
            _uiSlots.Add(ui);
        }
    }

    private void changeImageAndText(GameObject uiSlot, InventorySlot slot) {
        var itemDrag = uiSlot.GetComponentInChildren<ItemDrag>();
        if (itemDrag.Characteristics == null) {
            itemDrag.gameObject.GetComponent<Image>().sprite = slot.Characteristics.sprite;
            itemDrag.Characteristics = slot.Characteristics;
            itemDrag.onSpawn += onSpawn;
        }

        uiSlot.GetComponentInChildren<Text>().text = $"{slot.NumberAvailable}/{slot.NumberTotal}";
    }

    private void onSpawn(object sender, ItemDrag.SpawnEventArg e) {
        _inventory.callToMap(e.characteristics, e.tile);
    }

    private void showNewItem(object sender, InventorySlotEventArgs inventorySlotEventArgs) {
        var index = inventorySlotEventArgs.index;
        changeImageAndText(_uiSlots[index], inventorySlotEventArgs.inventorySlot);
    }
}
}