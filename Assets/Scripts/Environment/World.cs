using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public struct Tile
    {
        #region Attributes

        private int m_intensity;
        private int m_nextIntensity;

        public int Intensity
        {
            get => m_intensity;
            set { m_intensity = value; m_nextIntensity = value; }
        }

        public int NextIntensity => m_nextIntensity;


        public enum TileType
        {
            eNone = 0,
            eLake,
            ePlain,
            eRoad,
            eBuilding,
            eForest
        }
        public TileType m_type;

        public Tile(int p_intensity = 0, TileType p_type = TileType.eNone) { m_intensity = p_intensity; m_nextIntensity = p_intensity; m_type = p_type; }

        #endregion

        #region Probability stuff

        public void IntensifyNext(float p_externalProbability)
        {
            float proba = p_externalProbability * Probability4TileType;
            if (Random.value < Mathf.Abs(proba))
                m_nextIntensity += (int)Mathf.Sign(proba);
        }

        public float Probability4TileType
        {
            get => s_Probability4TileType[(int)m_type];
        }

        public float Probability4Intensity
        {
            get => Probability4Intensity_Func(m_intensity);
        }

        public float Probability4NextIntensity
        {
            get => Probability4Intensity_Func(m_nextIntensity);
        }

        #endregion

        #region Static probability stuff

        private static float[] s_Probability4TileType = { 0, 0, 1f/4, 1f/4, 1f/3, 1f/2 }; // N L P R B F
        private static float[] s_Probability4Intensity = { 0, 1f/3, 1f/2, 1, -1f/2 };

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
        public Vector2Int m_maxWorld = new Vector2Int(50, 30);
        public int m_physicalBeginning = 0;
        public bool m_verbose = false;
        private Tile[][] m_world;
        private static Tile nullTile;

        public ref Tile this[Vector2Int p_index]
        {
            get
            {
                Vector2Int indexArray = ToArrayIndex(p_index);
                if (indexArray.x >= 0 && indexArray.y >= 0
                    && indexArray.x < m_maxWorld.x && indexArray.y < m_maxWorld.y)
                {
                    return ref m_world[p_index.x][p_index.y];
                }
                return ref nullTile;
            }
        }
        #endregion

        #region Unity's message
        // Start is called before the first frame update
        void Start()
        {
            m_world = new Tile[m_maxWorld.x][];
            for (int x = 0; x < m_maxWorld.x; x++)
            {
                m_world[x] = new Tile[m_maxWorld.y];
                for (int y = 0; y < m_maxWorld.y; y++)
                {
                    m_world[x][y].m_type = Tile.TileType.eForest;
                }
            }

            m_world[5][5].Intensity = 3;

            Display();
        }

        public bool trigger = false;
        // Update is called once per frame
        void Update()
        {
            if (trigger)
            {
                trigger = false;

                Fire();
                Display();
            }
        }
        #endregion

        #region Fire !!!
        public void Fire()
        {
            // Update
            for (int x = 0; x < m_maxWorld.x; x++)
            {
                for (int y = 0; y < m_maxWorld.y; y++)
                {
                    FireOneTile(new Vector2Int(x,y));
                }
            }

            // Storing !
            for (int x = 0; x < m_maxWorld.x; x++)
            {
                for (int y = 0; y < m_maxWorld.y; y++)
                {
                    // we can't create humidity with fire only
                    if (m_world[x][y].NextIntensity < 0 && m_world[x][y].Intensity >= 0)
                    {
                        m_world[x][y].Intensity = 0;
                    }
                    // store nextIntensity in intensity
                    else
                    {
                        m_world[x][y].Intensity = Mathf.Clamp(m_world[x][y].NextIntensity, -4, 4);
                    }
                }
            }
        }

        private void FireOneTile(Vector2Int p_index)
        {
            float currentIntensityProbability = this[p_index].Probability4Intensity;

            if (currentIntensityProbability == 0)
                return;

            
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    Vector2Int neighbour = new Vector2Int(p_index.x + x, p_index.y + y);
                    float neighbourDistanceProbability = Probability4Distance(p_index, neighbour);
                    this[new Vector2Int(p_index.x + x, p_index.y + y)].IntensifyNext(neighbourDistanceProbability * currentIntensityProbability);
                }
            }
        }

        private float Probability4Distance(Vector2Int p_left, Vector2Int p_right)
        {
            Vector2Int leftContinuous = ToContinuousIndex(p_left);
            Vector2Int rightContinuous = ToContinuousIndex(p_right);
            int diffX = Mathf.Abs(leftContinuous.x - rightContinuous.x);
            int diffY = Mathf.Abs(leftContinuous.y - rightContinuous.y);
            if (diffX < 2 && diffY < 2)
                return 1f / (diffX + diffY);
            return 0;
        }

        private void Display()
        {
            if (! m_verbose) return;

            string display = string.Format("Tile {0}x{1}\n", m_maxWorld.x, m_maxWorld.y);
            for (int y = 0; y < m_maxWorld.y; y++)
            {
                string line = "";
                for (int x = 0; x < m_maxWorld.x; x++)
                {
                    if (m_world[x][y].Intensity > 0)
                    {
                        line += "_" + m_world[x][y].Intensity.ToString();
                    }
                    else if (m_world[x][y].Intensity < 0)
                    {
                        line += "_~";
                    }
                    else
                    {
                        line += "__";
                    }
                }
                display += line + "\n";
            }

            Debug.Log(display);
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