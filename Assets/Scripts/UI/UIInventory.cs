// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
public class UIInventory : MonoBehaviour {
    private Inventory _inventory;
    [SerializeField] private List<InventorySlot> slots;
    private List<GameObject> _uiSlots;
    [SerializeField] private GameObject inventoryUI;

    private void Awake() {
        _inventory = FindObjectOfType<Inventory>();
    }

    private void Start() {
        _inventory.InventoryChanged += showNewItem;
        _uiSlots = new List<GameObject>();
        for (var i = 0; i < _inventory.Items.Count; i++) {
            var inventorySlot = _inventory.Items[i];
            var ui = Instantiate(inventoryUI, transform);
            ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200 + i * 100, 50);
            changeImageAndText(ui, inventorySlot);
            _uiSlots.Add(ui);
        }
    }

    private void changeImageAndText(GameObject uiSlot, InventorySlot slot) {
        uiSlot.GetComponentsInChildren<Image>()
            .First(go => go.gameObject.transform.parent.gameObject != gameObject)
            .sprite = slot.Characteristics.sprite;
        uiSlot.GetComponentInChildren<Text>().text = $"{slot.NumberAvailable}/{slot.NumberTotal}";
    }

    private void showNewItem(object sender, InventorySlotEventArgs inventorySlotEventArgs) {
        var index = inventorySlotEventArgs.index;
        changeImageAndText(_uiSlots[index], inventorySlotEventArgs.inventorySlot);
    }
}
}