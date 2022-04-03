using UnityEngine;

public class GenerateTiles : MonoBehaviour
{
	// The prefab to instanciated
	public GameObject tilePrefab;

	// The number of tiles
	public int verticalCount;
	public int horizontalCount;
	// Tile size (square)
	public int tileSize;

	// The root object of all tiles
	private GameObject rootObjects = null;
	// The translation count
	private float globalTranslate = 0.0f;
	// The starting tiles index
	private int startIndex = 0;

	Environment.Tile.TileType getRandomTileType()
	{
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
	void Start()
	{
		Environment.World.Inst.InitWorld(new Vector2Int(horizontalCount, verticalCount));

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
				if (x == 0) Environment.World.Inst[position].m_type = Environment.Tile.TileType.eBuilding;
				
				instance.GetComponent<Environment.TileGraphic>().UpdateTile();
				instance.GetComponent<Environment.TileGraphic>().m_position = position;
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Update the translation
		globalTranslate -= 0.01f;
		
		// If one column of tile is outside
		if (globalTranslate <= -tileSize) {	
			// Reset the actual translation
			globalTranslate = 0.0f;

			// For each tile in the column
			for (int z = 0; z < verticalCount; ++z) {
				// Get the instance of the tile
				GameObject instance = GameObject.Find("/Tile Generator/" + tilePrefab.name + "_" + startIndex + "_" + z);
				instance.transform.position = new Vector3((horizontalCount - 1) * tileSize, 0, z);

				// WIP random tile type
				Vector2Int index = new Vector2Int(startIndex, z);
				if (z != 15 && z != 16 && startIndex != 0) {
					Environment.World.Inst[index].m_type = getRandomTileType();
				}
				Environment.World.Inst[index].Intensity = 0;
				instance.GetComponent<Environment.TileGraphic>().UpdateTile();
			}
			// Start index is incremented by one (cycling > horizontal count)
			startIndex++;
			if (startIndex >= horizontalCount) {
				startIndex = 0;
			}
			Environment.World.Inst.m_physicalBeginning = startIndex;
		} else {
			// Translate all tiles
			rootObjects.transform.Translate(new Vector3(-0.01f, 0, 0));
		}
	}
}
