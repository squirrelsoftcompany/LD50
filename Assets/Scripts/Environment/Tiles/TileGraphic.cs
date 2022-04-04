using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class TileGraphic : MonoBehaviour
    {
        public Vector2Int m_position;
        public FireGraphic m_fireGraphic;
        
        [Header("Prefabs")]
        // The prefabs to instanciated
        public GameObject m_forestPrefab;
        public GameObject m_plainPrefab;
        public GameObject m_buildingPrefab;
        public GameObject m_roadPrefab;
        public GameObject m_lakePrefab;

        [Header("Civilian")]
        public GameObject m_civilian;

	    private GameObject m_graphicInstance = null;

        public ref Tile tileData => ref World.Inst[m_position];

        // Start is called before the first frame update
        void Start()
        {
            UpdateTile();
        }

        public virtual void UpdateFire()
        {
            m_fireGraphic.UpdateFire(tileData.Intensity);

            Player.IMortal[] mortals = GetComponentsInChildren<Player.IMortal>();
            foreach (var mortal in mortals)
                mortal.newFireIntensity(tileData.Intensity);
        }

        public virtual void UpdateTile()
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

            UpdateFire();
            UpdateCivilian();
        }

        protected virtual void UpdateCivilian()
        {
            if (m_civilian)
                m_civilian.SetActive(tileData.HasCivilian);
        }
    }
}
