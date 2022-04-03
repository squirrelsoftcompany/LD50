using System.Collections.Generic;
using Shop;
using UnityEngine;

namespace UI {
[RequireComponent(typeof(Canvas), typeof(ItemsShop))]
public class UIItemsShop : MonoBehaviour {
    private Canvas _canvas;
    private ItemsShop _shop;

    // Start is called before the first frame update
    private void Start() {
        _canvas = GetComponent<Canvas>();
        _shop = GetComponent<ItemsShop>();
    }

    public void showShop(bool show) {
        if (show) {
            // take 2 or 3 choices
            List<ShopChoice> choices = _shop.fetchChoices();
            // todo(xavier) display them in the canvas
            Debug.Log($"available choices are {choices}");
        }

        _canvas.enabled = show;
    }
}
}