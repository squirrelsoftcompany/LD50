using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkTile", menuName = "ChunkTile")]

public class ChunkTile : ScriptableObject {
    private List<Environment.Tile.TileType> tiles;
    public int chunkHorizontalSize = 10;
    public int chunkVerticalSize = 10;

    // Init the chunk 
    public void Init(Environment.Tile.TileType defaultType = Environment.Tile.TileType.eNone) {
        tiles = new List<Environment.Tile.TileType>();
        for (int x = 0; x < chunkHorizontalSize; ++x) {
            for (int z = 0; z < chunkVerticalSize; ++z) {
                tiles.Add(defaultType);
            }
        }
    }

    // Set tile
    public void SetTile(Environment.Tile.TileType type, int x, int y) {
        if (x >= chunkHorizontalSize || x < 0) {
            Debug.LogWarning("X out of bound");
            return;
        }
        if (y >= chunkVerticalSize || y < 0) {
            Debug.LogWarning("Y out of bound");
            return;
        }
        int index = (x * chunkVerticalSize) + (y % chunkVerticalSize);
        tiles.Insert(index, type);
    }

    // Get tile
    public Environment.Tile.TileType GetTile(int x, int y) {
        if (x >= chunkHorizontalSize || x < 0) {
            Debug.LogWarning("X out of bound");
            return Environment.Tile.TileType.eNone;
        }
        if (y >= chunkVerticalSize || y < 0) {
            Debug.LogWarning("Y out of bound");
            return Environment.Tile.TileType.eNone;
        }
        int index = (x * chunkVerticalSize) + (y % chunkVerticalSize);
        return tiles[index];
    }
}
