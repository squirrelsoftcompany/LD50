using System.Collections.Generic;
using UnityEngine;

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
    public int borderChunkSize = 30;
    // The list of chunk
    public List<ChunkTile> chunkList = new List<ChunkTile>();
    


    // The root object of all tiles
	private GameObject rootObject = null;
    // Instance map
	private List<GameObject> graphicTileMap = new List<GameObject>();
	private List<GameObject> graphicTileMap_leftBorder = new List<GameObject>();
	private List<GameObject> graphicTileMap_rightBorder = new List<GameObject>();
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
        int chunkIndex = (int)Random.Range(0.0f, chunkList.Count);
        ChunkTile chunk = chunkList[chunkIndex];

        for (int x = xStart; x < xEnd; ++x) {
            for (int z = zStart; z < zEnd; ++z) {
                // Get data
                int index = (x * verticalCount) + (z % verticalCount);
                GameObject instance = graphicTileMap[index];
                Environment.TileGraphic script = instance.GetComponent<Environment.TileGraphic>();

                // World update
                ref Environment.Tile tile = ref Environment.World.Inst[script.m_position];
                tile.m_type = (z == roadZIndex_1 || z == roadZIndex_2) ? Environment.Tile.TileType.eRoad : chunk.GetTile(x % chunkSize, z % chunkSize);
                if (!firstInitialization)
                {
                    tile.Intensity = 0;
                    tile.ComputeHasCivilian();
                }

                // Instance update
                if (! firstInitialization) instance.transform.Translate(new Vector3(horizontalCount, 0, 0));
                script.UpdateTile();
            }
        }
    }

    void setBorderChunk(int xStart, bool p_isLeft = true, bool firstInitialization = false)
    {
        int xEnd = xStart + chunkSize;
        int zStart = 0;
        int zEnd = borderChunkSize;

        for (int x = xStart; x < xEnd; ++x)
        {
            for (int z = zStart; z < zEnd; ++z)
            {
                // Get data
                int index = (x * borderChunkSize) + (Mathf.Abs(z) % borderChunkSize);
                GameObject instance = p_isLeft ? graphicTileMap_leftBorder[index] : graphicTileMap_rightBorder[index];
                Environment.TileGraphic script = instance.GetComponent<Environment.TileGraphic>();

                // Instance update
                if (! firstInitialization) instance.transform.Translate(new Vector3(horizontalCount, 0, 0));
                script.UpdateTile();
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        // init var
        verticalCount = verticalChunkCount * chunkSize;
        horizontalCount = horizontalChunkCount * chunkSize;
        roadZIndex_1 = verticalCount / 2;
        roadZIndex_2 = roadZIndex_1 + 1;

        // Init the world
        Environment.World.Inst.IgniteWorld(new Vector2Int(horizontalCount, verticalCount));
        // Get the root object and translate to 0,0
        rootObject = GameObject.Find("/Tile Generator");
        rootObject.transform.Translate(new Vector3(horizontalCount / 2.0f, 0, verticalCount / 2.0f));

        // Instantiate all the tiles
        for (int x = 0; x < horizontalCount; ++x) {
            for (int z = 0; z < verticalCount; ++z) {
                GameObject instance = Instantiate(tilePrefab, rootObject.transform);
                instance.transform.position = new Vector3(x * tileSize, 0, z * tileSize);
                instance.name = tilePrefab.name + "_" + x + "_" + z;
                graphicTileMap.Add(instance);

                Vector2Int position = new Vector2Int(x, z);
                instance.GetComponent<Environment.TileGraphic>().m_position = position;
            }
        }
        // Instantiate all the left border
        for (int x = 0; x < horizontalCount; ++x) {
            for (int z = 0; z < borderChunkSize; ++z) {
                GameObject instance = Instantiate(borderTilePrefab, rootObject.transform);
                int trueZ = -z-1;
                instance.transform.position = new Vector3(x * tileSize, 0, trueZ * tileSize);
                instance.name = borderTilePrefab.name + "_" + x + "_" + trueZ;
                graphicTileMap_leftBorder.Add(instance);

                Vector2Int position = new Vector2Int(x, ((z+1)%verticalCount));
                instance.GetComponent<Environment.TileGraphic>().m_position = position;
            }
        }
        // Instantiate all the right border
        for (int x = 0; x < horizontalCount; ++x) {
            for (int z = 0; z < borderChunkSize; ++z) {
                GameObject instance = Instantiate(borderTilePrefab, rootObject.transform);
                int trueZ = verticalCount + z;
                instance.transform.position = new Vector3(x * tileSize, 0, trueZ * tileSize);
                instance.name = borderTilePrefab.name + "_" + x + "_" + trueZ;
                graphicTileMap_rightBorder.Add(instance);
                
                Vector2Int position = new Vector2Int(x, verticalCount - ((z+1)%verticalCount));
                instance.GetComponent<Environment.TileGraphic>().m_position = position;
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

    // Update is called once per frame
	void Update() {
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
			Environment.World.Inst.m_physicalBeginning = startIndex;
		} else {
			// Translate all tiles
            rootObject.transform.Translate(new Vector3(-scrollingSpeed * Time.deltaTime, 0, 0));
        }
	}
}
