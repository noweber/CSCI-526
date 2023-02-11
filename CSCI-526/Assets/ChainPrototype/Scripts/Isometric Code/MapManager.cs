using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public GameObject overlayPrefab;
    public GameObject overlayContainer;
    public GameObject unitContainer;
    [SerializeField] private GameObject _trianglePrefab;
    [SerializeField] private GameObject _diamondPrefab;
    [SerializeField] private GameObject _circlePrefab;

    [SerializeField] private GameObject _enemytrianglePrefab;
    [SerializeField] private GameObject _enemydiamondPrefab;
    [SerializeField] private GameObject _enemycirclePrefab;

    public Dictionary<Vector2Int, OverlayTile> map;
    public bool ignoreBottomTiles;
    public List<Vector3Int> playerLocations;
    public List<CharacterInfo> players;
    public List<Vector3Int> enemyLocations;
    public List<CharacterInfo> enemies;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        map = new Dictionary<Vector2Int, OverlayTile>();

        foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;

            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        if (z == 0 && ignoreBottomTiles)
                            return;

                        if (tm.HasTile(new Vector3Int(x, y, z)))
                        {
                            if (!map.ContainsKey(new Vector2Int(x, y)))
                            {
                                var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                                var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                                overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                                overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 1;
                                overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);
                                
                                map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                                if (x == 3 && y == 2 || x == 3 && y==-5)
                                {
                                    var enemyCircle = Instantiate(_enemycirclePrefab, overlayContainer.transform);
                                    enemyCircle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                    enemyCircle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                    enemyCircle.gameObject.GetComponent<CharacterInfo>().standingOnTile = overlayTile.gameObject.GetComponent<OverlayTile>();
                                    enemyLocations.Add(new Vector3Int(x, y, z));
                                    enemies.Add(enemyCircle.gameObject.GetComponent<CharacterInfo>());

                                }

                                if (x == -6 && y == 2 || x == -6 && y == -5)
                                {
                                    var circle = Instantiate(_circlePrefab, overlayContainer.transform);
                                    circle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                    circle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                    circle.gameObject.GetComponent<CharacterInfo>().standingOnTile = overlayTile.gameObject.GetComponent<OverlayTile>();
                                    playerLocations.Add(new Vector3Int(x, y, z));
                                    players.Add(circle.gameObject.GetComponent<CharacterInfo>());
                                }

                                if (x == 3 && y == -1 || x == 3 && y == -2)
                                {
                                    var enemyDiamond = Instantiate(_enemydiamondPrefab, overlayContainer.transform);
                                    enemyDiamond.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                    enemyDiamond.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                    enemyDiamond.gameObject.GetComponent<CharacterInfo>().standingOnTile = overlayTile.gameObject.GetComponent<OverlayTile>();
                                    enemyLocations.Add(new Vector3Int(x, y, z));
                                    enemies.Add(enemyDiamond.gameObject.GetComponent<CharacterInfo>());
                                }

                                if (x == -6 && y == -1 || x == -6 && y == -2)
                                {
                                    var diamond = Instantiate(_diamondPrefab, overlayContainer.transform);
                                    diamond.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                    diamond.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                    diamond.gameObject.GetComponent<CharacterInfo>().standingOnTile = overlayTile.gameObject.GetComponent<OverlayTile>();
                                    playerLocations.Add(new Vector3Int(x, y, z));
                                    players.Add(diamond.gameObject.GetComponent<CharacterInfo>());
                                }

                                if (x == 0 && y == 0 || x == 0 && y == -3)
                                {
                                    var enemyTriangle = Instantiate(_enemytrianglePrefab, overlayContainer.transform);
                                    enemyTriangle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                    enemyTriangle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                    enemyTriangle.gameObject.GetComponent<CharacterInfo>().standingOnTile = overlayTile.gameObject.GetComponent<OverlayTile>();
                                    enemyLocations.Add(new Vector3Int(x, y, z));
                                    enemies.Add(enemyTriangle.gameObject.GetComponent<CharacterInfo>());
                                }

                                if (x == -3 && y == 0 || x == -3 && y == -3)
                                {
                                    var triangle = Instantiate(_trianglePrefab, overlayContainer.transform);
                                    triangle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                    triangle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                    triangle.gameObject.GetComponent<CharacterInfo>().standingOnTile = overlayTile.gameObject.GetComponent<OverlayTile>();
                                    playerLocations.Add(new Vector3Int(x, y, z));
                                    players.Add(triangle.gameObject.GetComponent<CharacterInfo>());
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /*
        foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;

            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x; x < bounds.max.x; x++)
                    {
                        
                        if (x == 2 && y == 3)
                        {
                            var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, 50), Quaternion.identity);
                        }

                        if (x == 0 && y == 0)
                        {
                            var circle = Instantiate(_circlePrefab, new Vector3(x, y, 50), Quaternion.identity);
                        }

                        if (x == 3 && y == 0 || x == 4 && y == 0)
                        {
                            var diamond = Instantiate(_diamondPrefab, new Vector3(x, y, 50), _diamondPrefab.transform.rotation);
                        }
                        
                    }
                }
            }
        }
        */
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
    {
        var surroundingTiles = new List<OverlayTile>();


        Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        return surroundingTiles;
    }
}
