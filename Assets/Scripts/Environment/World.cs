using System;
using System.Collections.Generic;
using GameEventSystem;
using UnityEngine;
using Random = UnityEngine.Random;

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
            if (p_intensity >= 0 && p_intensity <= World.maxFireIntensity)
                return s_Probability4Intensity[p_intensity];
            return 0;
        }

        #endregion
    }

    public class World : MonoBehaviour
    {
        #region Attributes
        [Header("Logs")]
        public bool m_verbose = false;
        public bool m_logContinuous = false;
        
        [Header("Settings")]
        public int m_physicalBeginning = 0;
        public int m_continuousFireFrontLine = 0;
        public int m_absoluteFireFrontLine = 0;
        public static Tile nullTile;
        public float m_worldIsOnFire = 0; // Percent
        public static int maxFireIntensity = 4;
        public static int minFireIntensity = -4;
        [Header("Events")]
        public GameEvent m_intensificationDone;
        
        private Tile[][] m_world;
        private Vector2Int m_maxWorld = new Vector2Int(50, 30);
        private int m_fireIntensityMax = 0;
        
        // Singleton
        private static World instance = null;
        public static World Inst => instance;

        public ref Tile this[Vector2Int p_index]
        {
            get
            {
                Vector2Int indexArray = ToArrayIndex(p_index);
                if (indexArray.x >= 0 && indexArray.y >= 0
                    && indexArray.x < m_maxWorld.x && indexArray.y < m_maxWorld.y)
                {
                    return ref m_world[indexArray.x][indexArray.y];
                }
                return ref nullTile;
            }
        }
        #endregion

        #region Unity's message
        // Start is called before the first frame update
        void Start()
        {
            // init singleton
            if (Inst)
            {
                Debug.LogError("Class 'World' is a singleton !");
            }
            instance = this;
        }

        public void IgniteWorld(Vector2Int p_maxWorld)
        {
            m_maxWorld = p_maxWorld;
            m_world = null;

            // init world
            m_world = new Tile[m_maxWorld.x][];
            for (int x = 0; x < m_maxWorld.x; x++)
            {
                m_world[x] = new Tile[m_maxWorld.y];
                for (int y = 0; y < m_maxWorld.y; y++)
                {
                    m_world[x][y].m_type = Tile.TileType.eForest;
                }
            }

            m_world[5][p_maxWorld.y / 2].Intensity = 3;

            m_fireIntensityMax = (m_maxWorld.x * m_maxWorld.y) * 4;
        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        #region Fire !!!
        [ContextMenu("Fire !!!")]
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
            m_continuousFireFrontLine = 0;
            int fireIntensitySum = 0;
            for (int x = m_physicalBeginning; x < m_physicalBeginning + m_maxWorld.x; x++)
            {
                for (int y = 0; y < m_maxWorld.y; y++)
                {
                    Vector2Int continuousIndex = new Vector2Int(x, y);
                    ref Tile tile = ref this[continuousIndex];
                    // we can't create humidity with fire only
                    if (tile.NextIntensity < 0 && tile.Intensity >= 0)
                    {
                        tile.Intensity = 0;
                    }
                    // store nextIntensity in intensity
                    else
                    {
                        tile.Intensity = Mathf.Clamp(tile.NextIntensity, minFireIntensity, maxFireIntensity);
                    }

                    if (tile.Intensity > 0)
                    {
                        m_continuousFireFrontLine = Mathf.Max(m_continuousFireFrontLine, continuousIndex.x);
                        fireIntensitySum += tile.Intensity;
                    }
                }
            }
            m_worldIsOnFire = (float)fireIntensitySum / m_fireIntensityMax;
            // make m_fireFrontLine absolute
            m_absoluteFireFrontLine = (m_continuousFireFrontLine > m_maxWorld.x) ? m_continuousFireFrontLine - m_physicalBeginning : m_continuousFireFrontLine;

            // Notify
            m_intensificationDone?.Raise();

            Display();
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

                    Vector2Int neighbourIndex = new Vector2Int(p_index.x + x, p_index.y + y);
                    ref Tile neighbour = ref this[neighbourIndex];
                    if (neighbour.m_type != Tile.TileType.eNone)
                    {
                        float neighbourDistanceProbability = Probability4Distance(p_index, neighbourIndex);
                        neighbour.IntensifyNext(neighbourDistanceProbability * currentIntensityProbability);
                    }
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

            string display = string.Format("Tile {0}x{1}  WorldIsOnFire {2}  FrontLine {3}\n", m_maxWorld.x, m_maxWorld.y, m_worldIsOnFire, m_continuousFireFrontLine);
            for (int y = 0; y < m_maxWorld.y; y++)
            {
                string line = "";
                for (int x = 0; x < m_maxWorld.x; x++)
                {
                    Vector2Int index = new Vector2Int(m_logContinuous ? x + m_physicalBeginning : x, y);
                    ref Tile tile = ref this[index];
                    if (m_logContinuous)
                        line += (x == m_continuousFireFrontLine + 1) ? '|' : '_';
                    else
                        line += (x == m_absoluteFireFrontLine + 1) ? '|' : '_';
                    if (tile.Intensity > 0)
                    {
                        line += tile.Intensity.ToString();
                    }
                    else if (tile.Intensity < 0)
                    {
                        line += "~";
                    }
                    else
                    {
                        line += "_";
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
            int X = p_position.x;
            if (X < m_physicalBeginning)
                X = X + m_maxWorld.x;
            
            if (m_verbose && (X < m_physicalBeginning || X >= m_physicalBeginning + m_maxWorld.x))
                Debug.LogWarningFormat("ToContinuousIndex({0})={3} : X out of bounds [{1},{2}[", p_position.x, m_physicalBeginning, m_physicalBeginning + m_maxWorld.x, X);
            
            return new Vector2Int(X, p_position.y);
        }

        // Translate x-value to have array indices from continous indices
        // Already array index should not be affected
        private Vector2Int ToArrayIndex(Vector2Int p_position)
        {
            int X = p_position.x;
            if (X >= m_maxWorld.x && X < m_maxWorld.x + m_physicalBeginning)
                X = X - m_maxWorld.x;
            else if (X < 0 && X >= -m_physicalBeginning)
                X = X + m_maxWorld.x;

            if (m_verbose && (X < 0 || X >= m_maxWorld.x))
                Debug.LogWarningFormat("ToArrayIndex({0})={3} : X out of bounds [{1},{2}[", p_position.x, 0, m_maxWorld.x, X);
            
            return new Vector2Int(X, p_position.y);
        }
        #endregion

        public HashSet<Vector2Int> neighbours(Vector2Int position, float radius) {
            var sqrRadius = radius * radius;
            var res = new HashSet<Vector2Int>();
            for (var x = (int)(position.x - radius); x < position.x + radius; x++) {
                for (var y = (int)(position.y - radius); y < position.y + radius; y++) {
                    if (Math.Pow(position.x - x, 2) + Math.Pow(position.y - y, 2) > sqrRadius)
                        continue;
                    res.Add(new Vector2Int(x, y));
                }
            }

            return res;
        }
    }

}