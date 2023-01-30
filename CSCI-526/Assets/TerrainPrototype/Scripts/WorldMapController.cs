using Assets.TerrainPrototype.Source;
using System.Collections.Generic;
using UnityEngine;
using static Assets.TerrainPrototype.Source.TerrainMappings;

/// <summary>
/// A behavioral wrapper for controlling the world map data within the game.
/// </summary>
public class WorldMapController : MonoBehaviour
{
    public GameObject TerrainTilePrefab;

    public Vector2 TileSize = new(1.0f, 1.0f);

    public Vector2Int MapSize = new(10, 10);

    private IWorldMap worldMap;

    private List<GameObject> worldMapTiles;

    private void Awake()
    {
        worldMap = new WorldMap(MapSize.x, MapSize.y);
        worldMapTiles = new List<GameObject>();
    }

    private void Start()
    {
        // TODO: Remove these hard-coded 10 iterations and add/use a method on the WorldMap object to iterate generations until steady state.
        for (int i = 0; i < 100; i++)
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
                    if(tileController != null)
                    {
                        tileController.SetTileData(i, j, ref worldMap);
                    } else
                    {
                        Debug.LogError("Unable to get the terrain tile script when creating a tile in the world map.");
                    }
                    worldMapTiles.Add(nextTile);
                }
            }
        }
        else
        {
            Debug.LogError("The tile prefab is null, so the world map game objects can not be created in the scene.");
        }
    }
}
