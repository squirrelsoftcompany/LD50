using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public struct Tile
    {
        #region Attributes

        int m_intensity;
        int m_nextIntensity;

        public enum TileType
        {
            eNone = 0,
            eLake,
            ePlain,
            eRoad,
            eBuilding,
            eForest
        }
        TileType m_type;

        public Tile(int p_intensity = 0, TileType p_type = TileType.eNone) { m_intensity = p_intensity; m_nextIntensity = 0; m_type = p_type; }

        #endregion

        #region Probability stuff

        float Probability4TileType
        {
            get => s_Probability4TileType[(int)m_type];
        }

        float Probability4Intensity
        {
            get => Probability4Intensity_Func(m_intensity);
        }

        float Probability4NextIntensity
        {
            get => Probability4Intensity_Func(m_nextIntensity);
        }

        #endregion

        #region Static probability stuff

        private static float[] s_Probability4TileType = { 0, 0, 1f / 4, 1f / 4, 1f / 3, 1f / 2 }; // N L P R B F
        private static float[] s_Probability4Intensity = { 0, 1f / 3, 1f / 2, 1, -1f / 2 };

        private static float Probability4Intensity_Func(int p_intensity)
        {
            if (p_intensity >= 0 && p_intensity <= 4)
                return s_Probability4Intensity[p_intensity];
            return 0;
        }

        #endregion
    }

    public class World : MonoBehaviour
    {
        #region Attributes
        Vector2Int m_maxWorld = new Vector2Int(50, 30);
        private Tile[][] m_world;
        int m_physicalBeginning = 0;

        public Tile this[Vector2Int p_index]
        {
            get
            {
                Vector2Int indexArray = ToArrayIndex(p_index);
                if (indexArray.x >= 0 && indexArray.y >= 0
                    && indexArray.x < m_maxWorld.x && indexArray.y < m_maxWorld.y)
                {
                    return m_world[p_index.x][p_index.y];
                }
                return new Tile();
            }
        }
        #endregion

        #region Unity's message
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Fire !!!
        private float ProbabilityDistance(Vector2Int p_left, Vector2Int p_right)
        {
            Vector2Int leftContinuous = ToContinuousIndex(p_left);
            Vector2Int rightContinuous = ToContinuousIndex(p_right);
            int diffX = Mathf.Abs(leftContinuous.x - rightContinuous.x);
            int diffY = Mathf.Abs(leftContinuous.y - rightContinuous.y);
            if (diffX < 2 && diffY < 2)
                return 1 / (diffX + diffY);
            return 0;
        } 
        #endregion

        #region Index conversion
        // Translate x-value to have continuous indices from array indices
        // Already continuous index should not be affected
        private Vector2Int ToContinuousIndex(Vector2Int p_position)
        {
            if (p_position.x < m_physicalBeginning)
                return new Vector2Int((p_position.x + 1) + m_maxWorld.x, p_position.y);
            return p_position;
        }

        // Translate x-value to have array indices from continous indices
        // Already array index should not be affected
        private Vector2Int ToArrayIndex(Vector2Int p_position)
        {
            if (p_position.x > m_maxWorld.x)
                return new Vector2Int((p_position.x + 1) % m_maxWorld.x, p_position.y);
            return p_position;
        }
        #endregion
    }

}