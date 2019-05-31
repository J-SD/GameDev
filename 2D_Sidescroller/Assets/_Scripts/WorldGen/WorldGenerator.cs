using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class WorldGenerator : MonoBehaviour 
{
    public RuleTile groundTile;
    public Tilemap groundTilemap;
    public Tilemap plantTilemap;
    public Tilemap enemySpawnTilemap;
    public int difficultyLevel = 1;

    public EnemySpawnTile enemySpawnTile;

    public List<PlantTile> plantTiles = new List<PlantTile>();

    int chunkWidth = 32;
    int chunkHeight= 32;

    int horizontalChunks = 16;
    int verticalChunks = 1;
    int smoothingAmount = 4;

    int seed;



    List<Chunk> world;


    // Start is called before the first frame update
    void Start()
    {
        Regenerate();
    }

    public static int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }
        return map;
    }

    public void RenderChunk(Chunk chunkToRender)
    {
        for (int x = 0; x < chunkToRender.width; x++)
        {
            for (int y = 0; y < chunkToRender.height; y++)
            {
                Vector3Int location = new Vector3Int(x + chunkToRender.horizontalIndex * chunkToRender.width, y + chunkToRender.verticalIndex * chunkToRender.height, 0);
                if (chunkToRender.ground[x, y] == 1)
                {
                    groundTilemap.SetTile(location, groundTile);

                }
                if (chunkToRender.plants[x, y] == 1)
                {
                    PlantTile randomTile = plantTiles[Random.Range(0, plantTiles.Count)];
                    plantTilemap.SetTile(location, randomTile);

                }
                if (chunkToRender.enemies[x, y] == 1)
                {
                    enemySpawnTilemap.SetTile(location, enemySpawnTile);

                }
            }
        }


    }

    public Chunk GenerateChunk(int difficulty, float seed, int minSectionWidth, int chunkHorizontalIndex, int chunkVerticalIndex)
    {

        //Seed random
        System.Random rand = new System.Random(seed.GetHashCode());

        Chunk chunk = new Chunk(chunkWidth, chunkHeight, chunkHorizontalIndex, chunkVerticalIndex);


        int maxGroundHeight = Mathf.FloorToInt(chunkHeight / 2);


        //Determine the start position; the bottom of the range is the min height
        int lastHeight = Random.Range(3, maxGroundHeight);

        //Used to determine which direction to go
        int nextMove = 0;
        //Used to keep track of the current sections width
        int sectionWidth = 0;

        chunk.ground = GenerateArray(chunk.width, chunk.height, true);
        chunk.plants = GenerateArray(chunk.width, chunk.height, true);
        chunk.enemies = GenerateArray(chunk.width, chunk.height, true);

        //Work through the array width
        for (int x = 0; x < chunkWidth; x++)
        {
            //Determine the next move
            nextMove = rand.Next(2);

            //Only change the height if we have used the current height more than the minimum required section width
            if (nextMove == 0 && lastHeight > 0 && sectionWidth > minSectionWidth)
            {
                lastHeight--;
                sectionWidth = 0;
            }
            else if (nextMove == 1 && lastHeight < chunkHeight && sectionWidth > minSectionWidth)
            {
                lastHeight++;
                sectionWidth = 0;
            }
            //Increment the section width
            sectionWidth++;

            //GROUND

            //Work our way from the height down to 0
            for (int y = lastHeight; y >= 0; y--)
            {
                chunk.ground[x, y] = 1;
            }

            // PLANTS
            //check if lastheight+1 is in bounds and if there is a tile there
            // we can also use this to calculate where the exit should be 
            if (Random.Range(0, 10) < 8)
            { 
                int plantLocationY = lastHeight + 1;
                // make sure we're not underground
                if (chunk.ground[x, plantLocationY] != 1)
                {
                    chunk.plants[x, plantLocationY] = 1;
                }
            }

            // ENEMIES
            if (Random.Range(0, (20 / difficulty)) == 2)
            {
                int randomHeight = randomHeight = Random.Range(1, 5);
                // make sure it's a valid height for this chunk
                while (randomHeight + lastHeight >= chunk.height)
                {
                    randomHeight = Random.Range(1, 5);
                }

                int enemyY = lastHeight + randomHeight;

                if (chunk.ground[x, enemyY] != 1)
                {
                    chunk.enemies[x, enemyY] = 1;
                }
            }
        }

        return chunk;
    }

    void CreateChunks() {
        // loop through number of chunks:
        for (int cx = 0; cx < horizontalChunks; cx++)
        {
            for (int cy = 0; cy < verticalChunks; cy++)
            {
                world.Add(GenerateChunk(difficultyLevel, seed, smoothingAmount, cx, cy));
            }
        }
    }


    public void RenderWorld()
    {
        foreach (Chunk currentChunk in world)
        {
            RenderChunk(currentChunk);
        }

    }



    void Regenerate()
    {
        seed = Random.Range(1, 10000);
        world = new List<Chunk>();
        groundTilemap.ClearAllTiles();
        plantTilemap.ClearAllTiles();
        enemySpawnTilemap.ClearAllTiles();
        CreateChunks();
        RenderWorld();
    }

    public void UpdateWorld() {
           Regenerate();
    }
}
