using Player;
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
        public Civilian m_civilianPrefab;

	    private GameObject m_graphicInstance = null;
	    private Civilian m_civilianInstance = null;
	    public Civilian CivilianInstance => m_civilianInstance;
        
        private Tile tileData = null;
        public Tile TileData
        {
            get
            {
                if (tileData == null) tileData = World.Inst[m_position];
                return tileData;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        public virtual void UpdateFire()
        {
            m_fireGraphic.UpdateFire(TileData.Intensity);
        }

        public virtual void InitTile()
        {
            UpdateTile();
            UpdateFire();
            InitCivilian();
        }

        public virtual void UpdateTile()
        {
            Destroy(m_graphicInstance);
            switch
                (TileData.m_type)
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
        }

        protected virtual void InitCivilian()
        {
            if (m_civilianInstance) Destroy(m_civilianInstance);

            if (m_civilianPrefab && tileData.HasCivilian)
            {
                m_civilianInstance = Instantiate(m_civilianPrefab, transform);
            }
        }
    }
}
