using System.Collections.Generic;
using Shop;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
[RequireComponent(typeof(Canvas), typeof(ItemsShop))]
public class UIItemsShop : MonoBehaviour {
    private Canvas _canvas;
    private ItemsShop _shop;
    private List<GameObject> _uiSlots;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private int startX = -200;
    [SerializeField] private int intervalX = 200;
    [SerializeField] private int y = 300;

    public bool show = false;

    private float _formerTimeScale = 1f;
    // Start is called before the first frame update
    private void Start() {
        _canvas = GetComponent<Canvas>();
        _shop = GetComponent<ItemsShop>();
    }

    private void Update()
    {
            if(show)
            {
                showShop(show);
                show = false;
            }
    }

    public void showShop(bool show) {
        if (show) {
            _formerTimeScale = Time.timeScale;
            Time.timeScale = 0;
            // take 2 or 3 choices
            List<ShopChoice> choices = _shop.fetchChoices();
            _uiSlots = new List<GameObject>();
            for (var i = 0; i < choices.Count; i++)
            {
                var shopChoice = choices[i];
                var ui = Instantiate(shopUI, transform);
                ui.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(startX + i * intervalX, y);
                changeImageAndText(ui, shopChoice);
                _uiSlots.Add(ui);
            }
            Debug.Log($"available choices are {choices}");
        }
        else {
            Time.timeScale = _formerTimeScale;
        }

        _canvas.enabled = show;
    }

    private void changeImageAndText(GameObject uiSlot, ShopChoice shopChoice)
    {
        var itemClick = uiSlot.GetComponentInChildren<ItemClick>();
        if (itemClick.item == null)
        {
            itemClick.gameObject.GetComponent<Image>().sprite = shopChoice.Characteristics.sprite;
            itemClick.item = shopChoice;
            itemClick.onClick += onClick;
        }

        uiSlot.GetComponentInChildren<Text>().text = $"{shopChoice.Number}";
    }

    private void onClick(object sender, ItemClick.ClickEventArg e)
    {
        _shop.chooseShopPack(e.item);
        showShop(false); //Yes I know, raise an event is surly a better idea but I'm lazy
        for (int i = 0; i < _uiSlots.Count; i++)
        {
            Destroy(_uiSlots[i].gameObject);
        }
            _uiSlots.Clear();
    }

    }
}