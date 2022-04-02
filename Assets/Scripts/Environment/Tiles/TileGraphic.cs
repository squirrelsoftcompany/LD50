using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class TileGraphic : MonoBehaviour
    {
        public Vector2Int m_position;
        public GameObject m_firePivot;

        [Header("Colors")]
        // WIP the color of the tiles
        public Color treeColor;
        public Color plainColor;
        public Color houseColor;
        public Color roadColor;
        public Color lakeColor;

        // Start is called before the first frame update
        void Start()
        {
            UpdateTile();
            UpdateFire();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateFire()
        {
            Tile data = World.Inst[m_position];
            if (data.Intensity <= 0)
                m_firePivot.transform.localScale = Vector3.zero;
            else
                m_firePivot.transform.localScale = Vector3.one * data.Intensity / 4f;
        }

        public void UpdateTile()
        {
            Tile data = World.Inst[m_position];
            Color color = Color.magenta;
            switch
                (data.m_type)
            {
                case Tile.TileType.eForest:
                    color = treeColor;
                    break;
                case Tile.TileType.eBuilding:
                    color = houseColor;
                    break;
                case Tile.TileType.eRoad:
                    color = roadColor;
                    break;
                case Tile.TileType.ePlain:
                    color = plainColor;
                    break;
                case Tile.TileType.eLake:
                    color = lakeColor;
                    break;
            }
            gameObject.GetComponentInChildren<Renderer>().material.color = color;
        }
    }
}
