using UnityEngine;

public class GenerateEditor : MonoBehaviour
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

	// Start is called before the first frame update
	void Start()
	{
		// Get the root object and translate to 0,0
		rootObjects = GameObject.Find("/Editor Generator");
		rootObjects.transform.Translate(new Vector3(tileSize * horizontalCount / 2.0f, 0, tileSize * horizontalCount / 2.0f));

		// Create all tiles
		for (int x = 0; x < horizontalCount; ++x) {
			for (int y = 0; y < verticalCount; ++y) {
				
				// Get the new instance
				GameObject instance = Instantiate(tilePrefab, rootObjects.transform) as GameObject;
				// Set the position and the name
				instance.transform.position = new Vector3(x * tileSize, y * tileSize, 0);
				instance.transform.rotation = Quaternion.Euler(90, 0, 0);
				instance.name = tilePrefab.name + "_" + x + "_" + y;

				// WIP Get the mesh
				GameObject tileGameObject = GameObject.Find("/Editor Generator/" + instance.name + "/Tile");
				if (tileGameObject != null)
				{
					// Set the color of the mesh
					Material material = tileGameObject.GetComponent<MeshRenderer>().material;
					if (y == 15 || y == 16) {
						material.color = roadColor;
					} else {
						material.color = plainColor;
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

	}
}
