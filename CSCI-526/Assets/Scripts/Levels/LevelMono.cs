using Assets.Scripts.Levels;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelMono : MonoBehaviour
{
    public static LevelMono Instance { get; private set; }

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Circle _circlePrefab;

    [SerializeField] private Transform _camera;

    public Color playerColor;
    public Color enemyColor;

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
            }
        }

        foreach (var unit in units)
        {
            var coord = unit.GetPosition();
            if (unit.IsCircle())
            {
                var circle = Instantiate(_circlePrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                circle.SetName("Circle");
                circle.SetHuman(unit.IsHuman());
                circle.SetMoveState(false);
                circle.gameObject.GetComponent<SpriteRenderer>().color = circle.IsHuman() ? playerColor : enemyColor;
                _pieces[coord] = circle;
            }
            else if (unit.IsTriangle())
            {
                var triangle = Instantiate(_trianglePrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                triangle.SetName("Triangle");
                triangle.SetHuman(unit.IsHuman());
                triangle.SetMoveState(false);
                triangle.gameObject.GetComponent<SpriteRenderer>().color = triangle.IsHuman() ? playerColor : enemyColor;
                _pieces[coord] = triangle;
            }
            else
            {
                var diamond = Instantiate(_diamondPrefab, new Vector3(coord.Item1, coord.Item2, -1), Quaternion.identity);
                diamond.SetName("Diamond");
                diamond.SetHuman(unit.IsHuman());
                diamond.SetMoveState(false);
                diamond.gameObject.GetComponent<SpriteRenderer>().color = diamond.IsHuman() ? playerColor : enemyColor;
                _pieces[coord] = diamond;
            }
            Debug.Log(_pieces[coord].IsHuman());

        }

        _camera.transform.position = new Vector3((float)level.Width / 2 - 0.5f, (float)level.Height / 2 - 0.5f, -10);
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
