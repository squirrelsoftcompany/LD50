using System;
using System.Collections.Generic;
using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
public class FireFighter : Resource {
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