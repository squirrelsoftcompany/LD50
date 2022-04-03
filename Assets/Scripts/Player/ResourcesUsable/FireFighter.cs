using System;
using System.Collections.Generic;
using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
public class FireFighter : Resource {
    protected override void applyEffect() {
        HashSet<Vector2Int> neighbours =
            World.Inst.neighbours(Tile.m_position, Characteristics.rangeOfAction);

        foreach (var vector2Int in neighbours) {
            var neighbor = World.Inst[vector2Int];
            if (neighbor.m_type == Environment.Tile.TileType.eNone) continue;
            neighbor.Intensity =
                (int)Math.Clamp(neighbor.Intensity - Characteristics.efficiency,
                    World.minFireIntensity, World.maxFireIntensity);
            World.Inst[vector2Int].Intensity = neighbor.Intensity;
        }

        Debug.Log("applying effect " + Characteristics);
        // todo 
        // ! 
    }
}
}