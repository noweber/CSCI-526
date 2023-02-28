using Assets.Scripts.Levels;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMono : MonoBehaviour
{
    public static LevelMono Instance { get; private set; }

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Circle _circlePrefab;

    [SerializeField] private Scout _scoutPrefab;
    [SerializeField] private Camera _camera;

    public Color playerColor;
    public Color enemyColor;

    public Dictionary<Tuple<int, int>, Tile> tiles;
    public Dictionary<Tuple<int, int>, PieceMono> _pieces;

    public PieceMono selectedPiece = null;
    public Tuple<int, int> selectedCoord = new Tuple<int, int>(-1, -1);
    public List<Tuple<int, int>> highlightedMoves = new List<Tuple<int, int>>();

    public List<Tuple<int, int>> towerLocations = new List<Tuple<int, int>>();

    private int Width;

    private int Height;

    //[SerializeField] private GameObject fogObject;
    public bool debug;

    public bool capturedTower = false;       // For TutorialFogOfWar objective in MenuManager.cs
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
            //this.debug = false;
        }
    }

    public int GetWidth() { return this.Width; }

    public int GetHeight() { return this.Height; }

    public bool CheckOutOfBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

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

    public List<PieceMono> GetPlayerPieces()
    {
        List<PieceMono> playerPieces = new List<PieceMono>();
        foreach (var piece in _pieces)
        {
            if (piece.Value.IsHuman()) { playerPieces.Add(piece.Value); }
        }

        Debug.Log("NUM PLAYER PIECES " + playerPieces.Count);

        return playerPieces;
    }
    
    public List<PieceMono> GetEnemyPieces()
    {
        List<PieceMono> enemyPieces = new List<PieceMono>();
        foreach (var piece in _pieces)
        {
            if (!piece.Value.IsHuman()) { enemyPieces.Add(piece.Value); }
        }
        return enemyPieces;
    }

    public int GetEnemyPiecesNum()
    {
        return GetEnemyPieces().Count;
    }

    private void ClearVision()
    {
        foreach (var tile in tiles)
        {
            tile.Value.SetPlayerVisibility(false);
            tile.Value.SetEnemyVisibility(false);
            
            // Set all enemy pieces inactive 
            var coord = tile.Key;
            var piece = this.GetPiece(coord);
            if (piece != null && !piece.IsTriangle() && !piece.IsHuman())
            {
                piece.gameObject.SetActive(false);
            }
        }
    }

    private void RenderVision()
    {
        // set visible areas
        foreach (var p in this._pieces)
        {
            var piece = p.Value;
            var isHuman = piece.IsHuman();
            if (piece.IsTriangle())
            {
                if (isHuman) { this.SetPlayerVisibilityArea(piece.GetVisibleArea(2)); }
                if (!isHuman) { this.SetEnemyVisibilityArea(piece.GetVisibleArea(2)); }
            } 
            else if (piece.IsScout())
            {
                if (isHuman) { this.SetPlayerVisibilityArea(piece.GetVisibleArea(3)); }
                if (!isHuman) { this.SetEnemyVisibilityArea(piece.GetVisibleArea(3)); }
            }
            /*
            else if (piece.IsCircle())
            {
                if (isHuman) { this.SetPlayerVisibilityArea(piece.GetVisibleArea(1)); }
                if (!isHuman) { this.SetEnemyVisibilityArea(piece.GetVisibleArea(1)); }
            }
            */
        }
        
        // toggle fog and active pieces based on visible areas
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                var coord = new Tuple<int, int>(i, j);
                var tile = this.GetTile(coord);
                tile.ToggleFog();
                // tile.ToggleEnemyFog(); // Use this only for debugging
                
                // Set player/enemy pieces active if they are within range
                var piece = this.GetPiece(coord);
                if (tile.CanPlayerSee() && piece != null)
                {
                    piece.gameObject.SetActive(true);
                }
            }
        }
    }

    private void SetPlayerVisibilityArea(List<Tuple<int, int>> visibleArea)
    {
        foreach (var coord in visibleArea)
        {
            var x = coord.Item1;
            var y = coord.Item2;

            // sets fog
            if (this.debug == false)
            {
                tiles[coord].SetPlayerVisibility(true);
            }
        }
    }
    
    private void SetEnemyVisibilityArea(List<Tuple<int, int>> visibleArea)
    {
        foreach (var coord in visibleArea)
        {
            var x = coord.Item1;
            var y = coord.Item2;

            // sets fog
            if (this.debug == false)
            {
                tiles[coord].SetEnemyVisibility(true);
            }
        }
    }

    private void CreateSceneObjects(LoadLevelData level)
    {
        Debug.Log("check debug: " + this.debug);
        var units = level.Units;
        this.tiles = new Dictionary<Tuple<int, int>, Tile>();
        this._pieces = new Dictionary<Tuple<int, int>, PieceMono>();
        this.Width = level.Width;
        this.Height = level.Height;
        for (int x = 0; x < level.Width; x++)
        {
            for (int y = 0; y < level.Height; y++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, transform);
                tile.name = $"Tile {x} {y}";


                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tile.Init(isOffset);

                var coord = new Tuple<int, int>(x, y);
                tiles[coord] = tile;
                if (!this.debug)
                {
                    tile.SetPlayerVisibility(false);
                    tile.SetEnemyVisibility(false);
                }

            }
        }

        // Set what is visible to player
        foreach (var unit in units)
        {
            var coord = unit.GetPosition();
            var tile = this.tiles[coord];

            if (unit.IsTriangle())
            {
                var triangle = Instantiate(_trianglePrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                triangle.SetName("Triangle");
                triangle.SetHuman(unit.IsHuman());
                triangle.gameObject.SetActive(true);
                // if (!this.debug)
                // {
                //     this.SetRangeVisibility(triangle.GetVisibleArea(2), unit.IsHuman(), !unit.IsHuman());
                // }
                triangle.SetMoveState(false);
                triangle.gameObject.GetComponent<SpriteRenderer>().color = triangle.IsHuman() ? playerColor : enemyColor;
                _pieces[coord] = triangle;
                towerLocations.Add(coord);
            }
        }

        // Create units based on visibility
        foreach (var unit in units)
        {
            var coord = unit.GetPosition();
            var tile = this.tiles[coord];
            if (unit.IsCircle())
            {
                var circle = Instantiate(_circlePrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                circle.SetName(PieceMono.Circle);
                circle.SetHuman(unit.IsHuman());
                if (!this.debug)
                {
                    circle.gameObject.SetActive(unit.IsHuman());
                }
                circle.SetMoveState(false);
                circle.gameObject.GetComponent<SpriteRenderer>().color = circle.IsHuman() ? playerColor : enemyColor;
                _pieces[coord] = circle;
            }
            else if (unit.IsDiamond())
            {
                var diamond = Instantiate(_diamondPrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                diamond.SetName(PieceMono.Diamond);
                diamond.SetHuman(unit.IsHuman());
                if (!this.debug)
                {
                    diamond.gameObject.SetActive(unit.IsHuman());
                }
                //diamond.gameObject.SetActive(unit.IsHuman());
                diamond.SetMoveState(false);
                diamond.gameObject.GetComponent<SpriteRenderer>().color = diamond.IsHuman() ? playerColor : enemyColor;
                _pieces[coord] = diamond;
            } else if (unit.IsScout())
            {
                var scout = Instantiate(_scoutPrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                scout.SetName(PieceMono.Scout);
                scout.SetHuman(unit.IsHuman());
                if (!this.debug)
                {
                    scout.gameObject.SetActive(unit.IsHuman());
                }

                if (!unit.IsHuman())
                {
                    scout.SetInitialDirection(Direction.Down);
                }
                scout.SetMoveState(false);
                var squares = scout.gameObject.GetComponentsInChildren<SpriteRenderer>();
                foreach (var square in squares)
                {
                    square.color = scout.IsHuman() ? playerColor : enemyColor;
                }
                // if (!this.debug)
                // {
                //     this.SetRangeVisibility(scout.GetVisibleArea(3), unit.IsHuman(), !unit.IsHuman());
                // }
                _pieces[coord] = scout;
            }
        }
        
        // Sets Fog based on player vision
        // Also, defines what enemy can see as well
        if (!this.debug)
        {
            this.ClearVision();
            this.RenderVision();
        }

        _camera.transform.position = new Vector3((float)level.Width / 2 - 0.5f, (float)level.Height / 2 - 0.5f, -10);

        if (SceneManager.GetActiveScene().name == "TutorialFogOfWar")
        {
            _camera.orthographicSize = 6;
        }
        StartCoroutine(GameManagerChain.Instance.StateToHuman());
        // GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
    }

    public PieceMono GetPiece(Tuple<int, int> coord)
    {
        if (_pieces.TryGetValue(coord, out var piece))
        {
            return piece;
        }

        return null;
    }
    
    public Tile GetTile(Tuple<int, int> coord)
    {
        if (tiles.TryGetValue(coord, out var tile))
        {
            return tile;
        }

        return null;
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
        if (piece.IsHuman()) { this.highlightedMoves = piece.LegalMoves(this.Width, this.Height); }
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
            this.tiles[tileCoords].ShowVisibility();
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
            this.tiles[tileCoords].HideVisibility();
            //this.tiles[tileCoords]._highlight.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 100);
        }
    }

    public bool MovePiece(Tuple<int, int> coord)
    {
        var validMoves = this.selectedPiece.LegalMoves(this.Width, this.Height);
        bool currentPlayer = this.selectedPiece.IsHuman(); // true or false; human or AI
        if (!validMoves.Contains(coord))
        {
            return false;
        }

        this.selectedPiece.SetMoveState(true);
        var tile = this.tiles[coord];
        bool captured = false;
        if (this.GetPiece(coord) != null && this.GetPiece(coord).IsEnemyOf(this.selectedPiece))
        {
            // CAPTURE TAKES PLACE HERE
            // TODO: Implement intercepting capture
            Debug.Log("SOMETHING WAS CAPTURED");
            Destroy(this.GetPiece(coord).gameObject);
            //this.selectedPiece.gameObject.SetActive(true);
            captured = true;
            if (this.selectedPiece.IsCircle()) { this.selectedPiece.SetMoveState(false); }
        }
        if (this.selectedPiece.IsScout())
        {
            ((Scout)this.selectedPiece).SetDirection(coord);
        }
        
        this.selectedPiece.UpdateLocation(new Vector3(coord.Item1, coord.Item2, this.selectedPiece.transform.position.z));
        if(this.selectedPiece.IsHuman() && !this.selectedPiece.IsCircle())
        {
            if(this.selectedPiece.IsCircle())
            {
                if(captured)
                {
                    // Same green highlight
                }
                else
                {
                    this.selectedPiece.canMoveObject.SetActive(false);
                    this.selectedPiece.cantMoveObject.SetActive(true);
                }
            }
            else
            {
                this.selectedPiece.canMoveObject.SetActive(false);
                this.selectedPiece.cantMoveObject.SetActive(true);
            }
        }

        _pieces[coord] = this.selectedPiece;
        _pieces.Remove(selectedCoord);
        var towerCoord = this.selectedPiece.InTowerRange();
        if (towerCoord != null)
        {
            // CAPTURE TOWER
            if (SceneManager.GetActiveScene().name == "TutorialFogOfWar")
            {
                capturedTower = true;
            }
            Debug.Log("MOVED INTO RANGE OF ENEMY TOWER");
            var tower = _pieces[towerCoord];
            tower.SetHuman(currentPlayer);
            tower.gameObject.GetComponent<SpriteRenderer>().color = currentPlayer ? playerColor : enemyColor;
        }

        
        
        // Render vision
        if (this.debug == false)
        {
            if (captured)
            {
                this.selectedPiece.gameObject.SetActive(true);
            }
            else if (!this.selectedPiece.IsHuman() && tile.CanPlayerSee())
            {
                this.selectedPiece.gameObject.SetActive(true);
            }
            else if (!this.selectedPiece.IsHuman() && !tile.CanPlayerSee())
            {
                this.selectedPiece.gameObject.SetActive(false);
            }

            // set range visibilities for all player pieces
            this.ClearVision();
            this.RenderVision();
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
