using Assets.Scripts.Levels;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelMono : MonoBehaviour
{
    public static LevelMono Instance { get; private set; }

    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private TileBase _tile;

    public Tile _tilePrefab;
    public GameObject overlayPrefab;
    public GameObject overlayContainer;

    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Circle _circlePrefab;

    [SerializeField] private Triangle _enemytrianglePrefab;
    [SerializeField] private Diamond _enemydiamondPrefab;
    [SerializeField] private Circle _enemycirclePrefab;

    [SerializeField] private Transform _camera;

    public Color playerColor;
    public Color enemyColor;

    public List<PieceMono> enemies;
    public List<PieceMono> players;
    public List<Vector3Int> enemiesLocations;
    public List<Vector3Int> playersLocations;

    public Dictionary<Tuple<int, int>, Overlay> overlayTiles;
    public Dictionary<Tuple<int, int>, Tile> tiles;
    public Dictionary<Tuple<int, int>, PieceMono> _pieces;

    public PieceMono selectedPiece = null;
    public Tuple<int, int> selectedCoord = new Tuple<int, int>(-1, -1);
    public List<Tuple<int, int>> highlightedMoves = new List<Tuple<int, int>>();

    private int Width;

    private int Height;

    // public ILevel LevelModel { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public int GetWidth() { return this.Width; }

    public int GetHeight() { return this.Height; }

    public void LoadLevel(LoadLevelData level)
    {
        UnloadLevel();
        // LevelModel = new Level(level.Width, level.Height, level.Units);
        CreateSceneObjects(level);
    }

    private void UnloadLevel()
    {
        // TODO: Delete all tiles
        // TODO: Delete all pieces
        // TODO: Ensure any elements of the previous level are removed from the scene.
    }

    private void CreateSceneObjects(LoadLevelData level)
    {
        var units = level.Units;
        this.tiles = new Dictionary<Tuple<int, int>, Tile>();
        this._pieces = new Dictionary<Tuple<int, int>, PieceMono>();
        this.Width = level.Width;
        this.Height = level.Height;
        this.overlayTiles = new Dictionary<Tuple<int, int>, Overlay>();

        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);

        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                _tileMap.SetTile(new Vector3Int(y, x, 0), (TileBase)_tile);
                /*
                var tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, transform);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tile.Init(isOffset);

                var coord = new Tuple<int, int>(x, y);
                tiles[coord] = tile;
                */
            }
        }

        foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;
            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    if (tm.HasTile(new Vector3Int(y, x, 0)))
                    {
                        if (!tiles.ContainsKey(new Tuple<int, int>(y, x)))
                        {
                            var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                            var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(y, x, 0));
                            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 1;
                            overlayTile.gameObject.GetComponent<Overlay>().gridLocation = new Vector3Int(y, x, 0);

                            overlayTiles.Add(new Tuple<int, int>(y, x), overlayTile.gameObject.GetComponent<Overlay>());
                        }
                    }
                }
            }
        }
        foreach (var tm in tileMaps)
        {
            foreach (var unit in units)
            {
                var coord = unit.GetPosition();
                if (tm.HasTile(new Vector3Int(coord.Item1, coord.Item2, 0)))
                {
                    var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(coord.Item1, coord.Item2, 0));
                    var overlay = overlayTiles[new Tuple<int, int>(coord.Item1, coord.Item2)];
                    if (unit.IsCircle())
                    {
                        if (!unit.IsHuman())
                        {
                            var enemyCircle = Instantiate(_enemycirclePrefab, overlayContainer.transform);
                            enemyCircle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                            enemyCircle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                            enemyCircle.gameObject.GetComponent<PieceMono>().standingOnTile = overlay.gameObject.GetComponent<Overlay>();
                            enemyCircle.gameObject.GetComponent<PieceMono>().SetName("Circle");
                            enemyCircle.SetHuman(unit.IsHuman());
                            enemyCircle.SetMoveState(false);
                            enemies.Add(enemyCircle.gameObject.GetComponent<PieceMono>());
                            enemiesLocations.Add(enemyCircle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                            _pieces[coord] = enemyCircle;
                        }
                        else
                        {
                            var circle = Instantiate(_circlePrefab, overlayContainer.transform);
                            circle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                            circle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                            circle.gameObject.GetComponent<PieceMono>().standingOnTile = overlay.gameObject.GetComponent<Overlay>();
                            circle.gameObject.GetComponent<PieceMono>().SetName("Circle");
                            circle.SetHuman(unit.IsHuman());
                            circle.SetMoveState(false);
                            players.Add(circle.gameObject.GetComponent<PieceMono>());
                            playersLocations.Add(circle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                            _pieces[coord] = circle;
                        }
                    }
                    else if (unit.IsTriangle())
                    {
                        if (!unit.IsHuman())
                        {
                            var enemyTriangle = Instantiate(_enemytrianglePrefab, overlayContainer.transform);
                            enemyTriangle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                            enemyTriangle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                            enemyTriangle.gameObject.GetComponent<PieceMono>().standingOnTile = overlay.gameObject.GetComponent<Overlay>();
                            enemyTriangle.gameObject.GetComponent<PieceMono>().SetName("Triangle");
                            enemyTriangle.SetHuman(unit.IsHuman());
                            enemyTriangle.SetMoveState(false);
                            enemies.Add(enemyTriangle.gameObject.GetComponent<PieceMono>());
                            enemiesLocations.Add(enemyTriangle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                            _pieces[coord] = enemyTriangle;
                        }
                        else
                        {
                            var triangle = Instantiate(_trianglePrefab, overlayContainer.transform);
                            triangle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                            triangle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                            triangle.gameObject.GetComponent<PieceMono>().standingOnTile = overlay.gameObject.GetComponent<Overlay>();
                            triangle.gameObject.GetComponent<PieceMono>().SetName("Triangle");
                            triangle.SetHuman(unit.IsHuman());
                            triangle.SetMoveState(false);
                            players.Add(triangle.gameObject.GetComponent<PieceMono>());
                            playersLocations.Add(triangle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                            _pieces[coord] = triangle;
                        }
                    }
                    else
                    {
                        if (!unit.IsHuman())
                        {
                            var enemyDiamond = Instantiate(_enemydiamondPrefab, overlayContainer.transform);
                            enemyDiamond.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                            enemyDiamond.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                            enemyDiamond.gameObject.GetComponent<PieceMono>().standingOnTile = overlay.gameObject.GetComponent<Overlay>();
                            enemyDiamond.gameObject.GetComponent<PieceMono>().SetName("Diamond");
                            enemyDiamond.SetHuman(unit.IsHuman());
                            enemyDiamond.SetMoveState(false);
                            enemies.Add(enemyDiamond.gameObject.GetComponent<PieceMono>());
                            enemiesLocations.Add(enemyDiamond.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                            _pieces[coord] = enemyDiamond;
                        }
                        else
                        {
                            var diamond = Instantiate(_diamondPrefab, overlayContainer.transform);
                            diamond.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                            diamond.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                            diamond.gameObject.GetComponent<PieceMono>().standingOnTile = overlay.gameObject.GetComponent<Overlay>();
                            diamond.gameObject.GetComponent<PieceMono>().SetName("Diamond");
                            diamond.SetHuman(unit.IsHuman());
                            diamond.SetMoveState(false);
                            players.Add(diamond.gameObject.GetComponent<PieceMono>());
                            playersLocations.Add(diamond.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                            _pieces[coord] = diamond;
                        }
                    }
                    Debug.Log(_pieces[coord].IsHuman());
                }
            }
        }

        _camera.transform.position = new Vector3((float)level.Width / 6 - 0.5f, (float)level.Height / 3 - 0.5f, -10);
        GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
    }

    public PieceMono GetPiece(Tuple<int, int> coord)
    {
        if (_pieces.TryGetValue(coord, out var piece))
        {
            return piece;
        }

        return null;
    }

    public List<Tuple<int, int>> GetEnemyPieceCoords()
    {
        List<Tuple<int, int>> enemyPieces = new List<Tuple<int, int>>();
        Debug.Log(_pieces.Count);
        foreach (var piece in _pieces)
        {

            if (!piece.Value.IsHuman() && !piece.Value.IsTriangle()) { enemyPieces.Add(piece.Key); }
        }
        return enemyPieces;
    }

    public LevelMono()
    {
        Instance = this;
    }

    public bool HasSelectedPiece()
    {
        return this.selectedPiece != null;
    }

    public void SelectPiece(PieceMono piece, Tuple<int, int> coord)
    {
        this.selectedPiece = piece;
        this.selectedCoord = coord;
        if (piece.IsHuman()) { this.highlightedMoves = piece.LegalMoves(this.Height, this.Width); }
        if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1 && this.selectedPiece.IsCircle())
        {
            this.highlightedMoves.Add(new Tuple<int, int>(1, 1));
            this.highlightedMoves.Add(new Tuple<int, int>(2, 2));
        }
    }

    public void ResetPiece()
    {
        this.selectedCoord = new Tuple<int, int>(-1, -1);
        this.selectedPiece = null;
        this.highlightedMoves.Clear();
    }

    public void HighlightMoves()
    {
        foreach (Tuple<int, int> tileCoords in this.highlightedMoves)
        {
            this.tiles[tileCoords]._legal.SetActive(true);
            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && tileCoords.Item1 == 1 && tileCoords.Item2 == 1 && GameManagerChain.Instance.TotalMoves == 1) { LevelMono.Instance.tiles[tileCoords]._legal.GetComponent<SpriteRenderer>().color = new Color32(200, 100, 70, 255); }
            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && tileCoords.Item1 == 2 && tileCoords.Item2 == 2 && GameManagerChain.Instance.TotalMoves == 1) { LevelMono.Instance.tiles[tileCoords]._legal.GetComponent<SpriteRenderer>().color = new Color32(200, 100, 70, 255); }

            // Reset colors of the two tiles
            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && tileCoords.Item1 == 1 && tileCoords.Item2 == 1 && GameManagerChain.Instance.TotalMoves > 2) { LevelMono.Instance.tiles[tileCoords]._legal.GetComponent<SpriteRenderer>().color = new Color32(0, 185, 255, 100); }
            if (GameManagerChain.Instance.SceneName == "TutorialLevel" && tileCoords.Item1 == 2 && tileCoords.Item2 == 2 && GameManagerChain.Instance.TotalMoves > 2) { LevelMono.Instance.tiles[tileCoords]._legal.GetComponent<SpriteRenderer>().color = new Color32(0, 185, 255, 100); }

        }
    }

    public void RemoveHighlight()
    {
        foreach (Tuple<int, int> tileCoords in this.highlightedMoves)
        {
         	this.tiles[tileCoords]._legal.SetActive(false);
        	//this.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);
    	  }	
	  }

    public bool MovePiece(Tuple<int, int> coord)
    {
        var validMoves = this.selectedPiece.LegalMoves(this.Width, this.Height);
        if (!validMoves.Contains(coord))
        {
            return false;
        }

        this.selectedPiece.SetMoveState(true);
        if (this.GetPiece(coord) != null && this.GetPiece(coord).IsEnemyOf(this.selectedPiece)) 
        {
            // CAPTURE TAKES PLACE HERE
            Debug.Log("SOMETHING WAS CAPTURED");
            Destroy(this.GetPiece(coord).gameObject);
            if (this.selectedPiece.IsCircle()) { this.selectedPiece.SetMoveState(false); }
        }
        this.selectedPiece.UpdateLocation(new Vector3(coord.Item1, coord.Item2, this.selectedPiece.transform.position.z));
        _pieces[coord] = this.selectedPiece;
        _pieces.Remove(selectedCoord);
        
        if (this.selectedPiece.inTriangleRange())
        {
            // GIVE ANOTHER MOVE IF GETS IN RANGE OF TRIANGLE
            Debug.Log("MOVED INTO RANGE OF TRIANGLE");
            this.selectedPiece.SetMoveState(false);
            GameManagerChain.Instance.DecrementMoves(1);
        }

        this.RemoveHighlight();
        this.ResetPiece();
        return true;
    }

    public bool DoHumansRemain()
    {
        return GetNumberOfUnitsRemainingForPlayer(true) > 0;
    }

    public bool DoEnemiesRemain()
    {
        return GetNumberOfUnitsRemainingForPlayer(false) > 0;
    }

    /// <summary>
    /// This method return checks for the number of units which remain in the level for a given play.
    /// </summary>
    /// <param name="checkHumanPlayer">Whether to check the human player or the AI player.</param>
    /// <returns>Returns the number of units [0, n].</returns>
    private int GetNumberOfUnitsRemainingForPlayer(bool checkHumanPlayer)
    {
        if (_pieces == null)
        {
            return 0;
        }

        int result = (from piece in _pieces.Values where (piece.IsHuman() == checkHumanPlayer && !piece.IsTriangle()) select piece).Count();
        return result;
    }
}
