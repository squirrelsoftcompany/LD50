// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System;
using ScriptableObjects;

namespace Shop {
[Serializable]
public class ShopChoice {
    public ResourceCharacteristics Characteristics;
    public int Number;
    public float Profitability;

    public ShopChoice(ResourceCharacteristics characteristics, int number, float profitability) {
        Characteristics = characteristics;
        Number = number;
        Profitability = profitability;
    }
}
}