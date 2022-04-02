using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTiles : MonoBehaviour
{
	public GameObject tilePrefab;

	public Color treeColor;
	public Color plainColor;
	public Color houseColor;
	public Color roadColor;

	public int verticalCount;
	public int horizontalCount;
	public int tileSize;

	// Start is called before the first frame update
	void Start()
	{
		for (int x = 0; x < horizontalCount; ++x) {
			for (int z = 0; z < verticalCount; ++z) {
				GameObject instance = Instantiate(tilePrefab, new Vector3(x * tileSize, 0, z * tileSize), Quaternion.identity) as GameObject;
				instance.name = tilePrefab.name + "_" + x + "_" + z;

				GameObject tileGameObject = GameObject.Find("/" + instance.name + "/Tile");
				if (tileGameObject != null)
				{
					Material material = tileGameObject.GetComponent<MeshRenderer>().material;
					if (x == 15 || x == 16) {
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
