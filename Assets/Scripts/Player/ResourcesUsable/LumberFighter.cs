using Environment;
using UnityEngine;

namespace Player.ResourcesUsable {
public class LumberFighter : HumanFighter {
    [SerializeField] private int criticalFireSurvivable = 3;

    // Start is called before the first frame update
    protected override void applyEffect() {
        foreach (var vector2Int in neighbours) {
            ref var tile = ref World.Inst[vector2Int];
            if (tile.m_type != Environment.Tile.TileType.eForest) continue;
            tile.m_type = Environment.Tile.TileType.ePlain;
            TileGraphic tileGraphic = ChunkGenerator.Inst.get(vector2Int);
            tileGraphic.UpdateTile();
        }
    }

    public override int criticalAmountSurvivable() => criticalFireSurvivable;

    public override void doDie() {
        deathEvent.sentString = "Lumber fighter";
        deathEvent.Raise();
    }
}
}