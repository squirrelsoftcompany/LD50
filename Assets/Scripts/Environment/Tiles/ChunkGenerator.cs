using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Environment;

public class ChunkGenerator : MonoBehaviour
{
    // Scrolling speed
	public float scrollingSpeed = 0.01f;
    // Tile prefab
	public GameObject tilePrefab = null;
    // Border tile prefab
	public GameObject borderTilePrefab = null;
    // The tile size
    public float tileSize = 1;
    // Vertical chunck count
    public int verticalChunkCount = 3;
    // Horizontal chunck count
    public int horizontalChunkCount = 5;
    // The chunk size (square)
    public int chunkSize = 10;
    // Border (vertical) size
    public int leftBorderChunkSize = 30;
    public int rightBorderChunkSize = 30;
    // The list of chunk
    public List<ChunkTile> chunkList = new List<ChunkTile>();

    private static ChunkGenerator instance = null;
    public static ChunkGenerator Inst => instance;

    public TileGraphic get(Vector2Int p_index)
    {
        return graphicTileList.Find(tileGraphic => tileGraphic.m_position == p_index);
    }

    // The root object of all tiles
	private GameObject rootObject = null;
    // Instance map
	private List<TileGraphic> graphicTileList = new List<TileGraphic>();
	private List<TileGraphic> graphicTileList_leftBorder = new List<TileGraphic>();
	private List<TileGraphic> graphicTileList_rightBorder = new List<TileGraphic>();
    // The translation count
	private float globalTranslate = 0.0f;
	// The starting tiles index
	private int startIndex = 0;
    // Vertical count
	private int verticalCount;
	// Horizontal count
	private int horizontalCount;
    // Road Z index
    private int roadZIndex_1;
    private int roadZIndex_2;

    // Set chunk
    void setChunk(int xStart, int zStart, bool firstInitialization = false) {
        int xEnd = xStart + chunkSize;
        int zEnd = zStart + chunkSize;

        // Get random chunck
        int chunkIndex = Random.Range(0, chunkList.Count);
        ChunkTile chunk = chunkList[chunkIndex];

        for (int x = xStart; x < xEnd; ++x) {
            for (int z = zStart; z < zEnd; ++z) {
                // Get data
                int index = (x * verticalCount) + (z % verticalCount);
                TileGraphic tileGraphic = graphicTileList[index];
                
                // World update
                ref Environment.Tile tile = ref Environment.World.Inst[tileGraphic.m_position];
                tile.m_type = (z == roadZIndex_1 || z == roadZIndex_2) ? Environment.Tile.TileType.eRoad : chunk.GetTile(x % chunkSize, z % chunkSize);
                if (!firstInitialization)
                {
                    tile.Intensity = 0;
                    tile.ComputeHasCivilian();
                }

                // Instance update
                if (! firstInitialization) tileGraphic.gameObject.transform.Translate(new Vector3(horizontalCount, 0, 0));
                tileGraphic.InitTile();
            }
        }
    }

    void setBorderChunk(int xStart, bool p_isLeft = true, bool firstInitialization = false)
    {
        int xEnd = xStart + chunkSize;
        int zStart = 0;
        int borderChunkSize = p_isLeft ? leftBorderChunkSize : rightBorderChunkSize;
        int zEnd = borderChunkSize;

        for (int x = xStart; x < xEnd; ++x)
        {
            for (int z = zStart; z < zEnd; ++z)
            {
                // Get data
                int index = (x * borderChunkSize) + (Mathf.Abs(z) % borderChunkSize);
                TileGraphic tileGraphic = p_isLeft ? graphicTileList_leftBorder[index] : graphicTileList_rightBorder[index];

                // Instance update
                if (! firstInitialization) tileGraphic.gameObject.transform.Translate(new Vector3(horizontalCount, 0, 0));
                tileGraphic.InitTile();
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        // Singleton
        if (instance != null)
        {
            Debug.LogError("Generate Tiles should be a singleton!");
        }
        instance = this;

        // init var
        verticalCount = verticalChunkCount * chunkSize;
        horizontalCount = horizontalChunkCount * chunkSize;
        roadZIndex_1 = verticalCount / 2;
        roadZIndex_2 = roadZIndex_1 + 1;

        // Init the world
        World.Inst.SetupWorld(new Vector2Int(horizontalCount, verticalCount));
        // Get the root object and translate to 0,0
        rootObject = GameObject.Find("/Tile Generator");
        rootObject.transform.Translate(new Vector3(horizontalCount / 2.0f, 0, verticalCount / 2.0f));

        // Instantiate all the tiles
        for (int x = 0; x < horizontalCount; ++x) {
            for (int z = 0; z < verticalCount; ++z) {
                GameObject instance = Instantiate(tilePrefab, rootObject.transform);
                TileGraphic tileGraphic = instance.GetComponent<TileGraphic>();
                instance.transform.position = new Vector3(x * tileSize, 0, z * tileSize);
                instance.name = tilePrefab.name + "_" + x + "_" + z;
                
                Debug.Assert(tileGraphic);
                graphicTileList.Add(tileGraphic);

                Vector2Int position = new Vector2Int(x, z);
                tileGraphic.m_position = position;
            }
        }
        // Instantiate all the left border
        for (int x = 0; x < horizontalCount; ++x) {
            for (int z = 0; z < leftBorderChunkSize; ++z) {
                GameObject instance = Instantiate(borderTilePrefab, rootObject.transform);
                TileGraphic tileGraphic = instance.GetComponent<TileGraphic>();

                int trueZ = -z-1;
                instance.transform.position = new Vector3(x * tileSize, 0, trueZ * tileSize);
                instance.name = borderTilePrefab.name + "_" + x + "_" + trueZ;
                graphicTileList_leftBorder.Add(tileGraphic);

                Vector2Int position = new Vector2Int(x, ((z+1)%verticalCount));
                tileGraphic.m_position = position;
            }
        }
        // Instantiate all the right border
        for (int x = 0; x < horizontalCount; ++x) {
            for (int z = 0; z < rightBorderChunkSize; ++z) {
                GameObject instance = Instantiate(borderTilePrefab, rootObject.transform);
                TileGraphic tileGraphic = instance.GetComponent<TileGraphic>();

                int trueZ = verticalCount + z;
                instance.transform.position = new Vector3(x * tileSize, 0, trueZ * tileSize);
                instance.name = borderTilePrefab.name + "_" + x + "_" + trueZ;
                graphicTileList_rightBorder.Add(tileGraphic);
                
                Vector2Int position = new Vector2Int(x, verticalCount - (z%verticalCount) - 1);
                tileGraphic.m_position = position;
            }
        }

        // Initialize the tiles
        for (int xChunck = 0; xChunck < horizontalChunkCount; ++xChunck)
        {
            for (int zChunck = 0; zChunck < verticalChunkCount; ++zChunck)
            {
                setChunk(xChunck * chunkSize, zChunck * chunkSize, true);
            }
        }
        for (int xChunk = 0; xChunk < horizontalChunkCount; ++xChunk)
            setBorderChunk(xChunk * chunkSize, true, true);
        for (int xChunk = 0; xChunk < horizontalChunkCount; ++xChunk)
            setBorderChunk(xChunk * chunkSize, false, true);
    }

    public void ResetWorld()
    {
        World.Inst.ExtinguishWorld();
    }

    public void IgniteWorld()
    {
        World.Inst.IgniteWorld(new Vector2Int(15, (verticalCount / 2) - 1));
    }

    // Update is called once per frame
	void FixedUpdate() {
		// Update the translation
		globalTranslate -= scrollingSpeed * Time.deltaTime;
		
		// If one column of tile is outside
		if (globalTranslate <= -chunkSize) {	
			// Reset the actual translation
			globalTranslate = 0.0f;

			// For each chunk in the column
            for (int z = 0; z < verticalChunkCount; ++z) {
                setChunk(startIndex, z * chunkSize);
            }
            
            // For each left border chunk in the column
            setBorderChunk(startIndex);
            // For each right border chunk in the column
            setBorderChunk(startIndex, false);
            
			// Start index is incremented by one (cycling > horizontal count)
			startIndex += chunkSize;
			if (startIndex >= horizontalCount) {
				startIndex = 0;
			}
			World.Inst.m_physicalBeginning = startIndex;
		} else {
			// Translate all tiles
            rootObject.transform.Translate(new Vector3(-scrollingSpeed * Time.fixedDeltaTime, 0, 0));
        }
	}
}
