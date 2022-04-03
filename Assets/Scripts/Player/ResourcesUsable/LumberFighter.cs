using Environment;

namespace Player.ResourcesUsable {
public class LumberFighter : Resource {
    // Start is called before the first frame update
    protected override void applyEffect() {
        foreach (var vector2Int in neighbours) {
            ref var tile = ref World.Inst[vector2Int];
            if (tile.m_type != Environment.Tile.TileType.eForest) continue;
            tile.m_type = Environment.Tile.TileType.ePlain;
            TileGraphic tileGraphic = GenerateTiles.Inst.get(vector2Int);
            tileGraphic.UpdateTile();
        }
    }
}
}