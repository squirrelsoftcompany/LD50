using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class TileGraphic : MonoBehaviour
    {
        public Vector2Int m_position;
        public GameObject m_firePivot;

        [Header("Prefabs")]
        // The prefabs to instanciated
        public GameObject m_forestPrefab;
        public GameObject m_plainPrefab;
        public GameObject m_buildingPrefab;
        public GameObject m_roadPrefab;
        public GameObject m_lakePrefab;

	    private GameObject m_graphicInstance = null;

        // Start is called before the first frame update
        void Start()
        {
            UpdateTile();
            UpdateFire();
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

            Destroy(m_graphicInstance);
            switch
                (data.m_type)
            {
                case Tile.TileType.eForest:
                    m_graphicInstance = Instantiate(m_forestPrefab, transform) as GameObject;
                    break;
                case Tile.TileType.eBuilding:
                    m_graphicInstance = Instantiate(m_buildingPrefab, transform) as GameObject;
                    break;
                case Tile.TileType.eRoad:
                    m_graphicInstance = Instantiate(m_roadPrefab, transform) as GameObject;
                    break;
                case Tile.TileType.ePlain:
                    m_graphicInstance = Instantiate(m_plainPrefab, transform) as GameObject;
                    break;
                case Tile.TileType.eLake:
                    m_graphicInstance = Instantiate(m_lakePrefab, transform) as GameObject;
                    break;
            }
        }
    }
}
