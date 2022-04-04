using System.Collections.Generic;
using System.Linq;
using Environment;
using UnityEngine;

// Generated tile map
public class GenerateTiles : MonoBehaviour {
	// The prefabs to instanciated
	public GameObject tilePrefab;

	// Vertical count
	public int verticalCount;
	// Horizontal count
	public int horizontalCount;
	// Tile size (square)
	public float tileSize;
	// Scrolling speed
	public float scrollingSpeed = 0.01f;

	// The root object of all tiles
	private GameObject rootObjects = null;
	// The translation count
	private float globalTranslate = 0.0f;
	// The starting tiles index
	private int startIndex = 0;
	// Instance map
	private List<GameObject> graphicTileMap;

    private static GenerateTiles instance = null;
    public static GenerateTiles Inst => instance;

    public TileGraphic get(Vector2Int p_index) {
        return graphicTileMap.Select(obj => obj.GetComponent<TileGraphic>())
            .FirstOrDefault(tile => tile.m_position == p_index);
    }

    Environment.Tile.TileType getRandomTileType() {
        float random = Random.Range(0.0f, 1.0f);

		if (random < 0.66f)
		{
			return Environment.Tile.TileType.eForest;
		}
		else if (random < 0.70f)
		{
			return Environment.Tile.TileType.eBuilding;
		}
		else if (random < 0.75f)
		{
			return Environment.Tile.TileType.eLake;
		}
		return Environment.Tile.TileType.ePlain;
	}

	// Start is called before the first frame update
	void Start() {
        if (instance != null) {
            Debug.LogError("Generate Tiles should be a singleton!");
        }

        instance = this;
		World.Inst.SetupWorld(new Vector2Int(horizontalCount, verticalCount));
		World.Inst.IgniteWorld(new Vector2Int(15, (verticalCount/2) - 1));
		graphicTileMap = new List<GameObject>();

		// Get the root object and translate to 0,0
		rootObjects = GameObject.Find("/Tile Generator");
		rootObjects.transform.Translate(new Vector3(tileSize * horizontalCount / 2.0f, 0, tileSize * horizontalCount / 2.0f));

		// Create all tiles
		for (int x = 0; x < horizontalCount; ++x) {
			for (int z = 0; z < verticalCount; ++z) {
				
				// Get the new instance
				GameObject instance = Instantiate(tilePrefab, rootObjects.transform) as GameObject;
				// Set the position and the name
				instance.transform.position = new Vector3(x * tileSize, 0, z * tileSize);
				instance.name = tilePrefab.name + "_" + x + "_" + z;

				// WIP random tile type
				Vector2Int position = new Vector2Int(x, z);
				Environment.World.Inst[position].m_type = (z != 15 && z != 16)? getRandomTileType() : Environment.Tile.TileType.eRoad;
				graphicTileMap.Add(instance);
				
				instance.GetComponent<Environment.TileGraphic>().m_position = position;
				instance.GetComponent<Environment.TileGraphic>().UpdateTile();
			}
		}
	}

	// Update is called once per frame
	void Update() {
		// Update the translation
		globalTranslate -= scrollingSpeed;
		
		// If one column of tile is outside
		if (globalTranslate <= -tileSize) {	
			// Reset the actual translation
			globalTranslate = 0.0f;

			// For each tile in the column
			for (int z = 0; z < verticalCount; ++z) {
				// Get the instance and destroy instance
				int index = (startIndex * verticalCount) + (z % verticalCount);
				GameObject instance = graphicTileMap[index];
				Vector2Int newPosition = new Vector2Int(startIndex, z);

				// WIP random tile type
				if (z != 15 && z != 16) {
					Environment.World.Inst[newPosition].m_type = getRandomTileType();
				}
				Environment.World.Inst[newPosition].Intensity = 0;
				
				instance.transform.position = new Vector3((horizontalCount - 1) * tileSize, 0, z * tileSize);
				instance.GetComponent<Environment.TileGraphic>().m_position = newPosition;
				instance.GetComponent<Environment.TileGraphic>().UpdateTile();
				instance.GetComponent<Environment.TileGraphic>().UpdateFire(); // we need to force an UpdateFire now otherwise fire will seem to cycle
			}
			// Start index is incremented by one (cycling > horizontal count)
			startIndex++;
			if (startIndex >= horizontalCount) {
				startIndex = 0;
			}
			Environment.World.Inst.m_physicalBeginning = startIndex;
		} else {
			// Translate all tiles
			rootObjects.transform.Translate(new Vector3(-scrollingSpeed, 0, 0));
		}
	}
}
