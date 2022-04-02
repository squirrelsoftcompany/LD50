using UnityEngine;

public class GenerateTiles : MonoBehaviour
{
	// The prefab to instanciated
	public GameObject tilePrefab;

	// WIP the color of the tiles
	public Color treeColor;
	public Color plainColor;
	public Color houseColor;
	public Color roadColor;
	public Color lakeColor;

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

	Color getRandomColor() {
		float random = Random.Range(0.0f, 1.0f);

		if (random < 0.66f) {
			return treeColor;
		} else if (random < 0.70f) {
			return houseColor;
		} else if (random < 0.75f) {
			return lakeColor;
		}
		return plainColor;
	}

	// Start is called before the first frame update
	void Start()
	{
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

				// WIP Get the mesh
				GameObject tileGameObject = GameObject.Find("/Tile Generator/" + instance.name + "/Tile");
				if (tileGameObject != null)
				{
					// Set the color of the mesh
					Material material = tileGameObject.GetComponent<MeshRenderer>().material;
					if (z == 15 || z == 16) {
						material.color = roadColor;
					} else {
						material.color = getRandomColor();
					}
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

				// WIP swap color
				GameObject tileGameObject = GameObject.Find("/Tile Generator/" + tilePrefab.name + "_" + startIndex + "_" + z + "/Tile");
				Material material = tileGameObject.GetComponent<MeshRenderer>().material;
				if (z != 15 && z != 16) {
					material.color = getRandomColor();
				}
			}
			// Start index is incremented by one (cycling > horizontal count)
			startIndex++;
			if (startIndex >= horizontalCount) {
				startIndex = 0;
			}
		} else {
			// Translate all tiles
			rootObjects.transform.Translate(new Vector3(-0.01f, 0, 0));
		} 
	}
}
