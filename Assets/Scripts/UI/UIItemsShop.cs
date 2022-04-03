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
            Time.timeScale = 0;
            // take 2 or 3 choices
            List<ShopChoice> choices = _shop.fetchChoices();
            // todo(xavier) display them in the canvas
            Debug.Log($"available choices are {choices}");
            // todo(xavier) call (somewhere) _shop.chooseShopPack(myChoiceClicked);
        }
        else {
            Time.timeScale = 1;
        }

        _canvas.enabled = show;
    }
}
}