using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    // Scrolling speed
	public float scrollingSpeed = 0.01f;
    // Tile prefab
	public GameObject tilePrefab = null;
    // The tile size
    public float tileSize = 1;
    // Vertical chunck count
    public int verticalChunkCount = 3;
    // Horizontal chunck count
    public int horizontalChunkCount = 5;
    // The chunk size (square)
    public int chunkSize = 10;
    // The list of chunk
    public List<ChunkTile> chunkList = new List<ChunkTile>();
    


    // The root object of all tiles
	private GameObject rootObject = null;
    // Instance map
	private List<GameObject> graphicTileMap = new List<GameObject>();
    // The translation count
	private float globalTranslate = 0.0f;
	// The starting tiles index
	private int startIndex = 0;
    // Vertical count
	private int verticalCount;
	// Horizontal count
	private int horizontalCount;

    // Set chunk
    void setChunk(int xStart, int xEnd, int zStart, int zEnd) {
        // Get random chunck
        int chunkIndex = (int)Random.Range(0.0f, chunkList.Count);
        ChunkTile chunk = chunkList[chunkIndex];
        float random = Random.Range(0.0f, 1.0f);


        /*ChunkTile chunkInstance = chunk;

		if (random < 0.33f) {
			chunkInstance = chunk.flipHorizontal();
		} else if (random < 0.66f) 	{
			chunkInstance = chunk.flipVertical();
		} else {
			chunkInstance = chunk.rotate0();
		}*/

        for (int x = xStart; x < xEnd; ++x) {
            for (int z = zStart; z < zEnd; ++z) {
                // Get data
                int index = (x * verticalCount) + (z % verticalCount);
                
                GameObject instance = graphicTileMap[index];
                Vector2Int position = new Vector2Int(startIndex + x, z);

                // World update
                Environment.World.Inst[position].m_type = chunk.GetTile(x % chunkSize, z % chunkSize);
                Environment.World.Inst[position].Intensity = 0;

                // Instance update
                instance.transform.Translate(new Vector3(horizontalCount, 0, 0));
                instance.GetComponent<Environment.TileGraphic>().UpdateTile();
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        verticalCount = verticalChunkCount * chunkSize;
        horizontalCount = horizontalChunkCount * chunkSize;

        // Init the world
        Environment.World.Inst.IgniteWorld(new Vector2Int(horizontalCount, verticalCount));
        // Get the root object and translate to 0,0
        rootObject = GameObject.Find("/Tile Generator");
        rootObject.transform.Translate(new Vector3(horizontalCount / 2.0f, 0, verticalCount / 2.0f));

        int xOffset = 0;
        for(int xChunck = 0; xChunck < horizontalChunkCount; ++xChunck) {
            int zOffset = 0;
            for(int zChunck = 0; zChunck < verticalChunkCount; ++zChunck) {

                // Get random chunck
                int index = (int)Random.Range(0.0f, chunkList.Count);
                ChunkTile chunk = chunkList[index];
                
                // Create all tiles
                for(int x = 0; x < chunk.chunkHorizontalSize; ++x) {
                    for(int z = 0; z < chunk.chunkVerticalSize; ++z) {

                        int trueX = (x + xOffset);
                        int trueZ = (z + zOffset);

                        // Get the new instance
                        GameObject instance = Instantiate(tilePrefab, rootObject.transform) as GameObject;
                        // Set the position and the name
                        instance.transform.position = new Vector3(trueX * tileSize, 0, trueZ * tileSize);
                        instance.name = tilePrefab.name + "_" + trueX + "_" + trueZ;

                        // WIP random tile type
                        Vector2Int position = new Vector2Int(trueX, trueZ);
                        Environment.World.Inst[position].m_type = chunk.GetTile(x, z);
                        graphicTileMap.Add(instance);
                        
                        // Update tiles
                        instance.GetComponent<Environment.TileGraphic>().m_position = position;
                        instance.GetComponent<Environment.TileGraphic>().UpdateTile();
                    }
                }
                zOffset += chunkSize;
            }
            xOffset += chunkSize;
        }
    }

    // Update is called once per frame
	void Update() {
		// Update the translation
		globalTranslate -= scrollingSpeed;
		
		// If one column of tile is outside
		if (globalTranslate <= -chunkSize) {	
			// Reset the actual translation
			globalTranslate = 0.0f;

			// For each chunk in the column
            for (int z = 0; z < verticalChunkCount; ++z) {
                setChunk(startIndex, startIndex + chunkSize, z * chunkSize, (z * chunkSize) + chunkSize);
            }
            
			// Start index is incremented by one (cycling > horizontal count)
			startIndex += chunkSize;
			if (startIndex >= horizontalCount) {
				startIndex = 0;
			}
			Environment.World.Inst.m_physicalBeginning = startIndex;
		} else {
			// Translate all tiles
			rootObject.transform.Translate(new Vector3(-scrollingSpeed, 0, 0));
		}
	}
}
