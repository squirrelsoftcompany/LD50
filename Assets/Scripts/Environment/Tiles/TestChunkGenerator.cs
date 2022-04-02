using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChunkGenerator : MonoBehaviour
{
	public GameObject tilePrefab;
    public ChunkTile chunk;
    
    // The root object of all tiles
	private GameObject rootObjects = null;

    Color getColor(Environment.Tile.TileType type) {
        switch(type) {
            case Environment.Tile.TileType.eLake:
                return Color.blue;
            case Environment.Tile.TileType.ePlain:
                return new Color(0.0f, 192.0f / 255.0f, 110.0f / 255.0f, 1.0f);
            case Environment.Tile.TileType.eRoad:
                return Color.grey;
            case Environment.Tile.TileType.eBuilding:
                return Color.magenta;
            case Environment.Tile.TileType.eForest:
                return Color.green;
        }
        return Color.black;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the root object and translate to 0,0
		rootObjects = GameObject.Find("/Tile Generator");
		rootObjects.transform.Translate(new Vector3(chunk.chunkHorizontalSize / 2.0f, 0, chunk.chunkVerticalSize / 2.0f));

        for(int x = 0; x < chunk.chunkHorizontalSize; ++x) {
            for(int z = 0; z < chunk.chunkVerticalSize; ++z) {
                // Get the new instance
				GameObject instance = Instantiate(tilePrefab, rootObjects.transform) as GameObject;
				
                // Set the position and the name
				instance.transform.position = new Vector3(x, 0, z);
				instance.name = tilePrefab.name + "_" + x + "_" + z;

				// WIP Get the mesh
				GameObject tileGameObject = GameObject.Find("/Tile Generator/" + instance.name + "/Tile");
				if (tileGameObject != null)
				{
					// Set the color of the mesh
					Material material = tileGameObject.GetComponent<MeshRenderer>().material;
					material.color = getColor(chunk.GetTile(x, z));
				}
				else
				{
					Debug.LogWarning("Tile is null");
				}
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
