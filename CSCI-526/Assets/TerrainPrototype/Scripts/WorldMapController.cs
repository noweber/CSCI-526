using Assets.TerrainPrototype.Source;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A behavioral wrapper for controlling the world map data within the game.
/// </summary>
public class WorldMapController : MonoBehaviour
{
    public static WorldMapController Instance;

    public GameObject TerrainTilePrefab;

    public Vector2 TileSize = new(1.0f, 1.0f);

    public Vector2Int MapSize = new(10, 10);

    public int Age = 10;

    private IWorldMap worldMap;

    private List<TerrainTile> worldMapTiles;

    public TerrainTile GetRandomSpawnTile()
    {
        return worldMapTiles.Where(x => x.Occupant == null).OrderBy(t => Random.value).First();
    }

    public void CreateWorldMap(Vector2Int mapSize)
    {
        if (worldMapTiles != null)
        {
            foreach (TerrainTile tile in worldMapTiles)
            {
                Destroy(tile.transform.gameObject);
            }
        }
        MapSize = mapSize;
        worldMap = new WorldMap(MapSize.x, MapSize.y);
        worldMapTiles = new List<TerrainTile>();

        for (int i = 0; i < Age; i++)
        {
            worldMap.GotoNextGeneration();
        }

        // Create a the map tiles using the prefab:
        if (TerrainTilePrefab != null)
        {
            for (int i = 0; i < worldMap.GetMapWidth(); i++)
            {
                for (int j = 0; j < worldMap.GetMapHeight(); j++)
                {
                    GameObject nextTile = Instantiate(TerrainTilePrefab, new Vector3(i * TileSize.x, -j * TileSize.y, 0) + transform.position, TerrainTilePrefab.transform.rotation, transform);
                    TerrainTile tileController = nextTile.GetComponent<TerrainTile>();
                    if (tileController != null)
                    {
                        tileController.SetTileData(i, j, ref worldMap);
                    }
                    else
                    {
                        Debug.LogError("Unable to get the terrain tile script when creating a tile in the world map.");
                    }
                    worldMapTiles.Add(tileController);
                }
            }
        }
        else
        {
            Debug.LogError("The tile prefab is null, so the world map game objects can not be created in the scene.");
        }
    }

    private void Awake()
    {
        Instance = this;
        CreateWorldMap(MapSize);
    }
}
