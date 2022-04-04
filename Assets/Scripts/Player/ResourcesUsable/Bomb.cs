// Project Assembly-CSharp
// 
// Created by jessi on 2022-04-02

using System;
using System.Collections;
using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
[RequireComponent(typeof(Animator))]
public class Bomb : Resource {
    private Animator _animator;
    private static readonly int Boom = Animator.StringToHash("Boom");

    protected new void Awake() {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    protected override IEnumerator spawnAnimation() {
        yield return null;
    }

    protected override IEnumerator showActive() {
        _animator.SetTrigger(Boom);
        yield return null;
    }

    protected override void applyEffect() {
        foreach (var vector2Int in neighbours) {
            ref var neighbor = ref World.Inst[vector2Int];
            if (neighbor.m_type == Environment.Tile.TileType.eNone) continue;
            neighbor.Intensity =
                (int)Math.Clamp(neighbor.Intensity - Characteristics.efficiency,
                    World.minFireIntensity, World.maxFireIntensity);
        }
    }
}
}