using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EditorTileBehavior : MonoBehaviour
{
    private GameObject instance = null;
    private bool over = false;
    public Environment.Tile.TileType type = Environment.Tile.TileType.ePlain;

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
        instance = GameObject.Find("/Editor Generator/" + name + "/Tile");
        instance.GetComponent<MeshRenderer>().material.color = getColor(type);
    }

    void OnMouseOver()
    {
        instance.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
        over = true;
    }

    void OnMouseExit()
    {
        instance.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        over = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse input
        if (Input.GetMouseButtonDown(0) && over) {
            type++;
            if ((int)type >= Enum.GetNames(typeof(Environment.Tile.TileType)).Length) {
                type = Environment.Tile.TileType.eLake;
            }
            instance.GetComponent<MeshRenderer>().material.color = getColor(type);
        } else if (Input.GetMouseButtonDown(1) && over) {
            type--;
            if ((int)type <= 0) {
                type = Environment.Tile.TileType.eForest;
            }
            instance.GetComponent<MeshRenderer>().material.color = getColor(type);
        }
    }
}
