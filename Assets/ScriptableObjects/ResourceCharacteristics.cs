﻿// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using UnityEngine;

namespace ScriptableObjects {
[CreateAssetMenu(fileName = "InventoryItemCharacteristics", menuName = "New Inventory item")]
public class ResourceCharacteristics : ScriptableObject {
    public bool consumable;
    public int spawnWait = 2;
    public int activeDuration = 1;
    public int cooldown = 3;
    public float rangeOfAction = 1f;
    public float efficiency = 1f;
    public GameObject prefab;
}
}