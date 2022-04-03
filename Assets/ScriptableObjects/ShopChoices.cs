// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-03

using System.Collections.Generic;
using Shop;
using UnityEngine;

namespace ScriptableObjects {
[CreateAssetMenu(fileName = "ShopChoices", menuName = "All shop choices", order = 3)]
public class ShopChoices : ScriptableObject {
    [SerializeField] public List<ShopChoice> choices;
}
}