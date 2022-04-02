// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using UnityEngine;

namespace Player.ResourcesUsable {
public class Bomb : Resource {
    protected override void applyEffect() {
        Debug.Log("applying effect bomb");
    }
}
}