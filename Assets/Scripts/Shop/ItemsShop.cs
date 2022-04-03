// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System.Collections.Generic;
using System.Linq;
using Attributes;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Shop {
public class ItemsShop : MonoBehaviour {
    [SerializeField] private ShopChoices shopChoices;

    [ReadOnly] [SerializeField] private int shopOpenedTimes = 0;
    [SerializeField] private float profitabilityFactor = 1f;
    private Inventory _inventory;

    private void Awake() {
#if UNITY_EDITOR
        if (shopChoices == null) {
            Debug.LogError("ItemsShop should be completed with shopChoices");
        }
#endif
        _inventory = FindObjectOfType<Inventory>();
    }


    public List<ShopChoice> fetchChoices() {
        shopOpenedTimes++;
        var maxProfit = maxProfitability(shopOpenedTimes);
        var choices = shopChoices.choices.Where(choice => choice.Profitability <= maxProfit)
            .ToList();
        var nb = nbOfChoices(shopOpenedTimes);
        var res = new List<ShopChoice>();
        for (var i = 0; i < nb; i++) {
            var index = Random.Range(0, choices.Count);
            res.Add(choices[index]);
            choices.RemoveAt(index);
        }

        return res;
    }

    private float maxProfitability(int openTimes) {
        return Mathf.Log(openTimes) * profitabilityFactor;
    }

    private static int nbOfChoices(int openTimes) {
        if (openTimes < 10) return 2;
        return 3;
    }

    public void chooseShopPack(ShopChoice choice) {
        _inventory.choosePack(choice);
    }
}
}