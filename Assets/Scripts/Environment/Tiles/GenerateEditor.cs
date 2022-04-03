using System;
using UnityEngine;
using UnityEditor;

// Generate the tile map for the editor
public class GenerateEditor : MonoBehaviour {
	// The prefab to instanciated
	public GameObject tilePrefab;
	// Vertical count
	public int verticalCount;
	// Horizontal count
	public int horizontalCount;
	// Tile size (square)
	public float tileSize;

	// The root object of all tiles
	private GameObject rootObjects = null;

	// Start is called before the first frame update
	void Start() {
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
			}
		}
	}

	public void save() {
		// Create and set chunk
		ChunkTile asset = ScriptableObject.CreateInstance<ChunkTile>();
		asset.Init();
		for (int x = 0; x < horizontalCount; ++x) {
			for (int y = 0; y < verticalCount; ++y) {
				GameObject tile = GameObject.Find("/Editor Generator/" + tilePrefab.name + "_" + x + "_" + y);
				EditorTileBehavior script = tile.GetComponent<EditorTileBehavior>();
				asset.SetTile(script.type, x, y);
			}
		}

		// Set the name using the current date
		String date = ("" + System.DateTime.Now).Replace('/', '-').Replace(' ', '_').Replace(':', '-');
		AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/Chunks/" + date + ".asset");
		AssetDatabase.SaveAssets();
	}
}
