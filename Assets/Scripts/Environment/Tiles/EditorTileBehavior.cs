using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class implement the behavior of the tiles in the editor
public class EditorTileBehavior : MonoBehaviour {
	// The instance
	private GameObject instance = null;
	// True if the mouse is on over
	private bool onOver = false;
	// The type of the tile
	public Environment.Tile.TileType type = Environment.Tile.TileType.ePlain;

	// Editor colors
	Color getColor(Environment.Tile.TileType type) {
		switch(type) {
			case Environment.Tile.TileType.eLake:
				return Color.blue;
			case Environment.Tile.TileType.ePlain:
				return new Color(0.0f, 190.0f / 255.0f, 110.0f / 255.0f, 1.0f);
			case Environment.Tile.TileType.eRoad:
				return Color.grey;
			case Environment.Tile.TileType.eBuilding:
				return Color.magenta;
			case Environment.Tile.TileType.eForest:
				return new Color(0.0f, 70.0f / 255.0f, 15.0f / 255.0f, 1.0f);
		}
		// Default or eNone
		return Color.black;
	}

	// Start is called before the first frame update
	void Start() {
		instance = GameObject.Find("/Editor Generator/" + name + "/Tile");
		instance.GetComponent<MeshRenderer>().material.color = getColor(type);
	}

	
	// Update is called once per frame
	void Update() {
		// Get mouse input
		if (Input.GetMouseButtonDown(0) && onOver) {
			
			// Next tile type
			type++;
			if ((int)type >= Enum.GetNames(typeof(Environment.Tile.TileType)).Length) {
				type = Environment.Tile.TileType.eLake;
			}
			instance.GetComponent<MeshRenderer>().material.color = getColor(type);
		} else if (Input.GetMouseButtonDown(1) && onOver) {

			// Previous tile type
			type--;
			if ((int)type <= 0) {
				type = Environment.Tile.TileType.eForest;
			}
			instance.GetComponent<MeshRenderer>().material.color = getColor(type);
		}
	}

	// On over
	void OnMouseOver() {
		// Set emission to true
		instance.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
		onOver = true;
	}

	// On exit
	void OnMouseExit() {
		// Set emission to false
		instance.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
		onOver = false;
	}
}
