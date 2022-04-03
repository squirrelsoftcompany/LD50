using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChunkTile", menuName = "ChunkTile")]

public class ChunkTile : ScriptableObject {
    [SerializeField]
    public List<Environment.Tile.TileType> tiles;
    [SerializeField]
    public int chunkHorizontalSize = 10;
    [SerializeField]
    public int chunkVerticalSize = 10;
    [SerializeField]
	public float tileSize = 1.0f;

    // Init the chunk 
    public void Init(List<Environment.Tile.TileType> clone) {
        tiles = new List<Environment.Tile.TileType>();
        for (int x = 0; x < chunkHorizontalSize; ++x) {
            for (int z = 0; z < chunkVerticalSize; ++z) {
                int index = (x * chunkVerticalSize) + (z % chunkVerticalSize);
                tiles.Add(clone[index]);
            }
        }
    }

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

    public ChunkTile rotate0() {
        ChunkTile instance = ScriptableObject.CreateInstance<ChunkTile>();
        instance.Init(tiles);
        return instance;
    }

    public ChunkTile rotate90() {
        return null;
    }

    public ChunkTile rotate180() {
        return null;
    }

    public ChunkTile rotate270() {
        return null;
    }

    public ChunkTile flipVertical() {
        ChunkTile instance = ScriptableObject.CreateInstance<ChunkTile>();
        instance.Init(tiles);

        for (int x = 0; x < (int)Math.Floor(chunkHorizontalSize / 2.0f); ++x) {
            for (int z = 0; z < chunkVerticalSize; ++z) {
                Environment.Tile.TileType swapA = instance.GetTile(x, z);
                Environment.Tile.TileType swapB = instance.GetTile(chunkHorizontalSize - x - 1, z) ;
                
                instance.SetTile(swapB, x, z);
                instance.SetTile(swapA, x, chunkVerticalSize - z - 1);

            }
        }
        return instance;    
    }

    public ChunkTile flipHorizontal() {
        ChunkTile instance = ScriptableObject.CreateInstance<ChunkTile>();
        instance.Init(tiles);

        for (int x = 0; x < chunkHorizontalSize; ++x) {
            for (int z = 0; z < (int)Math.Floor(chunkVerticalSize / 2.0f); ++z) {
                Environment.Tile.TileType swapA = instance.GetTile(x, z);
                Environment.Tile.TileType swapB = instance.GetTile(x, chunkVerticalSize - z - 1) ;
                
                instance.SetTile(swapB, x, z);
                instance.SetTile(swapA, x, chunkVerticalSize - z - 1);
            }
        }
        return instance;
    }
}
