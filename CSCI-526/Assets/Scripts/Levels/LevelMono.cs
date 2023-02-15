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

    public Tile tilePrefab;
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
        this.overlayTiles = new Dictionary <Tuple<int, int>, Overlay>();

        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);

        for (int y = 0; y < level.Height; y++)
        {
            for (int x = 0; x < level.Width; x++)
            {
                _tileMap.SetTile(new Vector3Int(y, x, 0), (TileBase)_tile);
                //var y1 = y - 6;
                //var x1 = x - 5;
                //Debug.Log("" + y + ", " + x + ", " + 0);
            }
        }


        foreach (var tm in tileMaps)
        {
            /*
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

            foreach (var unit in units)
            {
                var coord = unit.GetPosition();
                var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(coord.Item1, coord.Item2, 0));
                if (unit.IsCircle())
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(coord.Item1, coord.Item2, cellWorldPosition.z + 2), Quaternion.identity);
                    circle.SetName("Circle");
                    circle.SetHuman(unit.IsHuman());
                    circle.SetMoveState(false);
                    circle.gameObject.GetComponent<SpriteRenderer>().color = circle.IsHuman() ? playerColor : enemyColor;
                    if (circle.IsHuman())
                    {
                        players.Add(circle);
                        playersLocations.Add(circle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                    } else
                    {
                        enemies.Add(circle);
                        enemiesLocations.Add(circle.standingOnTile.gridLocation);
                    }
                    _pieces[coord] = circle;
                }
                else if (unit.IsTriangle())
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(coord.Item1, coord.Item2, cellWorldPosition.z + 2), Quaternion.identity);
                    triangle.SetName("Triangle");
                    triangle.SetHuman(unit.IsHuman());
                    triangle.SetMoveState(false);
                    triangle.gameObject.GetComponent<SpriteRenderer>().color = triangle.IsHuman() ? playerColor : enemyColor;
                    if (triangle.IsHuman())
                    {
                        players.Add(triangle);
                        playersLocations.Add(triangle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                    }
                    else
                    {
                        enemies.Add(triangle);
                        enemiesLocations.Add(triangle.standingOnTile.gridLocation);
                    }
                    _pieces[coord] = triangle;
                }
                else
                {
                    var diamond = Instantiate(_diamondPrefab, new Vector3(coord.Item1, coord.Item2, cellWorldPosition.z + 2), Quaternion.identity);
                    diamond.SetName("Diamond");
                    diamond.SetHuman(unit.IsHuman());
                    diamond.SetMoveState(false);
                    diamond.gameObject.GetComponent<SpriteRenderer>().color = diamond.IsHuman() ? playerColor : enemyColor;
                    if (diamond.IsHuman())
                    {
                        players.Add(diamond);
                        playersLocations.Add(diamond.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                    }
                    else
                    {
                        enemies.Add(diamond);
                        enemiesLocations.Add(diamond.standingOnTile.gridLocation);
                    }
                    _pieces[coord] = diamond;
                }
                Debug.Log(_pieces[coord].IsHuman());

            }
        }
        */
            //Debug.Log(tm);

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
                            //_camera.transform.position = new Vector3((float)level.Width / 2 - 0.5f, (float)level.Height / 2 - 0.5f, -10);
                            if (x == 7 && y == 9 || x == 0 && y == 9)
                            {
                                var enemyCircle = Instantiate(_enemycirclePrefab, overlayContainer.transform);
                                enemyCircle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                enemyCircle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                enemyCircle.gameObject.GetComponent<PieceMono>().standingOnTile = overlayTile.gameObject.GetComponent<Overlay>();
                                enemyCircle.gameObject.GetComponent<PieceMono>().SetName("Circle");
                                enemyCircle.SetHuman(false);
                                //enemyLocations.Add(new Vector3Int(x, y, z));
                                enemies.Add(enemyCircle.gameObject.GetComponent<PieceMono>());
                                enemiesLocations.Add(enemyCircle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                                _pieces.Add(new Tuple<int, int>(x, y), enemyCircle);

                            }

                            if (x == 0 && y == 0 || x == 7 && y == 0)
                            {
                                var circle = Instantiate(_circlePrefab, overlayContainer.transform);
                                circle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                circle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                circle.gameObject.GetComponent<PieceMono>().standingOnTile = overlayTile.gameObject.GetComponent<Overlay>();
                                circle.gameObject.GetComponent<PieceMono>().SetName("Circle");
                                circle.SetHuman(true);
                                //playerLocations.Add(new Vector3Int(x, y, z));
                                players.Add(circle.gameObject.GetComponent<PieceMono>());
                                playersLocations.Add(circle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                                _pieces.Add(new Tuple<int, int>(x, y), circle);
                            }

                            if (x == 3 && y == 9 || x == 4 && y == 9)
                            {
                                var enemyDiamond = Instantiate(_enemydiamondPrefab, overlayContainer.transform);
                                enemyDiamond.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                enemyDiamond.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                enemyDiamond.gameObject.GetComponent<PieceMono>().standingOnTile = overlayTile.gameObject.GetComponent<Overlay>();
                                enemyDiamond.gameObject.GetComponent<PieceMono>().SetName("Diamond");
                                enemyDiamond.SetHuman(false);
                                //enemyLocations.Add(new Vector3Int(x, y, z));
                                enemies.Add(enemyDiamond.gameObject.GetComponent<PieceMono>());
                                enemiesLocations.Add(enemyDiamond.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                                _pieces.Add(new Tuple<int, int>(x, y), enemyDiamond);
                            }

                            if (x == 3 && y == 0 || x == 4 && y == 0)
                            {
                                var diamond = Instantiate(_diamondPrefab, overlayContainer.transform);
                                diamond.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                diamond.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                diamond.gameObject.GetComponent<PieceMono>().standingOnTile = overlayTile.gameObject.GetComponent<Overlay>();
                                diamond.gameObject.GetComponent<PieceMono>().SetName("Diamond");
                                diamond.SetHuman(true);
                                //playerLocations.Add(new Vector3Int(x, y, z));
                                players.Add(diamond.gameObject.GetComponent<PieceMono>());
                                playersLocations.Add(diamond.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                                _pieces.Add(new Tuple<int, int>(x, y), diamond);
                            }

                            if (x == 2 && y == 6 || x == 5 && y == 6)
                            {
                                var enemyTriangle = Instantiate(_enemytrianglePrefab, overlayContainer.transform);
                                enemyTriangle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                enemyTriangle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                enemyTriangle.gameObject.GetComponent<PieceMono>().standingOnTile = overlayTile.gameObject.GetComponent<Overlay>();
                                enemyTriangle.gameObject.GetComponent<PieceMono>().SetName("Triangle");
                                enemyTriangle.SetHuman(false);
                                //enemyLocations.Add(new Vector3Int(x, y, z));
                                enemies.Add(enemyTriangle.gameObject.GetComponent<PieceMono>());
                                enemiesLocations.Add(enemyTriangle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                                _pieces.Add(new Tuple<int, int>(x, y), enemyTriangle);
                            }

                            if (x == 2 && y == 3 || x == 5 && y == 3)
                            {
                                var triangle = Instantiate(_trianglePrefab, overlayContainer.transform);
                                triangle.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 2);
                                triangle.GetComponent<SpriteRenderer>().sortingOrder = tm.GetComponent<TilemapRenderer>().sortingOrder + 2;
                                triangle.gameObject.GetComponent<PieceMono>().standingOnTile = overlayTile.gameObject.GetComponent<Overlay>();
                                triangle.gameObject.GetComponent<PieceMono>().SetName("Triangle");
                                triangle.SetHuman(true);
                                //playerLocations.Add(new Vector3Int(x, y, z));
                                players.Add(triangle.gameObject.GetComponent<PieceMono>());
                                playersLocations.Add(triangle.gameObject.GetComponent<PieceMono>().standingOnTile.gridLocation);
                                _pieces.Add(new Tuple<int, int>(x, y), triangle);
                            }
                        }
                    }
                }
            }
        }
            
        
        foreach (var x in players){
            Debug.Log("" + x.GetName() + ": " + x.standingOnTile.gridLocation);
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
        //Debug.Log(_pieces.Count);
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
        if (piece.IsHuman()) { this.highlightedMoves = piece.LegalMoves(this.Height, this.Width);}
        if (GameManagerChain.Instance.SceneName == "TutorialLevel" && GameManagerChain.Instance.TotalMoves == 1 && this.selectedPiece.IsCircle())
        {
            this.highlightedMoves.Add(new Tuple<int, int>(1, 1));
            this.highlightedMoves.Add(new Tuple<int, int>(2, 2));
        }
        //foreach (var highlight in this.highlightedMoves)
        //{
        //    Debug.Log("Should Highlight: " + highlight);
        //}
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
            //Debug.Log("SOMETHING WAS CAPTURED");
            Destroy(this.GetPiece(coord).gameObject);
            if (this.selectedPiece.IsCircle()) { this.selectedPiece.SetMoveState(false); }
        }
        this.selectedPiece.UpdateLocation(new Vector3(coord.Item1, coord.Item2, this.selectedPiece.transform.position.z));
        _pieces[coord] = this.selectedPiece;
        _pieces.Remove(selectedCoord);

        if (this.selectedPiece.inTriangleRange())
        {
            // GIVE ANOTHER MOVE IF GETS IN RANGE OF TRIANGLE
            //Debug.Log("MOVED INTO RANGE OF TRIANGLE");
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
