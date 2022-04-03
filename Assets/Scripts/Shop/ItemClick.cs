using System.Collections;
using System.Collections.Generic;
using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClick : MonoBehaviour
{
    public Shop.ShopChoice item { get; set; }

    public event EventHandler<ClickEventArg> onClick;

    public void onMouseClick()
    {
        onClick?.Invoke(this, new ClickEventArg(item));
    }

    public class ClickEventArg : EventArgs
    {
        public Shop.ShopChoice item;

        public ClickEventArg(Shop.ShopChoice pItem)
        {
            this.item = pItem;
        }
    }
}
