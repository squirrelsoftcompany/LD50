using System;
using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
public class FireFighter : HumanFighter {
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] private int criticalFireSurvivable = 5;

    protected override void applyEffect() {
        foreach (var vector2Int in neighbours) {
            ref var neighbor = ref World.Inst[vector2Int];
            if (neighbor.m_type == Environment.Tile.TileType.eNone) continue;
            neighbor.Intensity =
                (int)Math.Clamp(neighbor.Intensity - Characteristics.efficiency,
                    World.minFireIntensity, World.maxFireIntensity);
        }
    }

    public override int criticalAmountSurvivable() => criticalFireSurvivable;
    public override void doDie() {
        deathEvent.sentString = "Fire fighter";
        deathEvent.Raise();
    }
}
}