using Assets.Scripts.Levels;
using Assets.Scripts.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] public int _width = 6, _height = 6;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Circle _circlePrefab;

    [SerializeField] private Transform _camera;

    public Color colorOne;
    public Color colorTwo;

    public Dictionary<Tuple<int, int>, Tile> tiles;
    public Dictionary<Tuple<int, int>, Piece> _pieces;

    public Piece storedPiece = null;
    public Tuple<int, int> storedCoord = new Tuple<int, int>(-1, -1);
    //public List<Vector2> storedValidMoves = storedPiece.LegalMoves(_width, _height) ? null;

    public ILevelModel levelModel;

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

        // TODO: Take these units (along with a width and height) from a file for each level to load.

        tiles = new Dictionary<Tuple<int, int>, Tile>();

        Dictionary<Tuple<int, int>, Tuple<bool, UnitType>> units = new();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tuple<int, int> position = new(x, y);
                if (x == 2 && y == 3 || x == _width - 3 && y == 3)
                {
                    units.Add(position, new Tuple<bool, UnitType>(true, UnitType.Triangle));
                }

                if (x == 0 && y == 0 || x == _width - 1 && y == 0)
                {
                    units.Add(position, new Tuple<bool, UnitType>(true, UnitType.Circle));
                }

                if (x == 3 && y == 0 || x == 4 && y == 0)
                {
                    units.Add(position, new Tuple<bool, UnitType>(true, UnitType.Diamond));
                }

                if (x == 2 && y == _height - 4 || x == _width - 3 && y == _height - 4)
                {
                    units.Add(position, new Tuple<bool, UnitType>(false, UnitType.Triangle));

                }

                if (x == 0 && y == _height - 1 || x == _width - 1 && y == _height - 1)
                {
                    units.Add(position, new Tuple<bool, UnitType>(false, UnitType.Circle));
                }

                if (x == 3 && y == _height - 1 || x == 4 && y == _height - 1)
                {
                    units.Add(position, new Tuple<bool, UnitType>(false, UnitType.Diamond));
                }
            }
        }
        levelModel = new LevelModel(_width, _height, units);
        Debug.Log("Units Dictionary");
        foreach (var pair in units)
        {
            Debug.Log("" + pair.Key + ", " + pair.Value);
        }
    }

    // TODO: Turn this into create scene objects
    public void GenerateGrid()
    {
        tiles = new Dictionary<Tuple<int, int>, Tile>();
        _pieces = new Dictionary<Tuple<int, int>, Piece>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tile.Init(isOffset);

                var coord = new Tuple<int, int>(x, y);
                tiles[coord] = tile;

                if (x == 2 && y == 3 || x == _width - 3 && y == 3)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    triangle.isWhite = true;
                    triangle.hasMoved = false;
                    triangle.gameObject.GetComponent<SpriteRenderer>().color = colorOne;
                    _pieces[coord] = triangle;

                }

                if (x == 0 && y == 0 || x == _width - 1 && y == 0)
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    circle.isWhite = true;
                    circle.hasMoved = false;
                    circle.gameObject.GetComponent<SpriteRenderer>().color = colorOne;
                    _pieces[coord] = circle;
                }

                if (x == 3 && y == 0 || x == 4 && y == 0)
                {
                    var diamond = Instantiate(_diamondPrefab, new Vector3(x, y, -1), _diamondPrefab.transform.rotation);
                    diamond.isWhite = true;
                    diamond.hasMoved = false;
                    diamond.gameObject.GetComponent<SpriteRenderer>().color = colorOne;
                    _pieces[coord] = diamond;
                }

                if (x == 2 && y == _height - 4 || x == _width - 3 && y == _height - 4)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    triangle.isWhite = false;
                    triangle.hasMoved = false;
                    triangle.gameObject.GetComponent<SpriteRenderer>().color = colorTwo;
                    _pieces[coord] = triangle;

                }

                if (x == 0 && y == _height - 1 || x == _width - 1 && y == _height - 1)
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    circle.isWhite = false;
                    circle.hasMoved = false;
                    circle.gameObject.GetComponent<SpriteRenderer>().color = colorTwo;
                    _pieces[coord] = circle;
                }

                if (x == 3 && y == _height - 1 || x == 4 && y == _height - 1)
                {
                    var diamond = Instantiate(_diamondPrefab, new Vector3(x, y, -1), _diamondPrefab.transform.rotation);
                    diamond.isWhite = false;
                    diamond.hasMoved = false;
                    diamond.gameObject.GetComponent<SpriteRenderer>().color = colorTwo;
                    _pieces[coord] = diamond;
                }
            }
        }

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        GameManagerChain.Instance.ChangeState(GameStateEnum.White);
        Debug.Log("Tiles Dictionary");
        foreach(var pair in tiles)
        {
            Debug.Log("" + pair.Key + ", " + pair.Value);
        }
    }

    public Tile GetTile(Tuple<int, int> coord)
    {
        if (tiles.TryGetValue(coord, out var tile))
        {
            return tile;
        }

        return null;
    }

    public Piece GetPiece(Tuple<int, int> coord)
    {
        if (_pieces.TryGetValue(coord, out var piece))
        {
            return piece;
        }

        return null;
    }

    public GridManager()
    {
        Instance = this;
    }

    public bool MovePiece(Tuple<int, int> coord, Piece piece)
    {
        var validMoves = piece.LegalMoves(_width, _height);
        if (!validMoves.Contains(new Tuple<int, int> (coord.Item1, coord.Item2)))
        {
            return false;
        }
        if (GridManager.Instance.GetPiece(coord) != null && piece.unitName == "Triangle")
        {
            Debug.Log("Return False Capture Triangle: " + coord);
            return false;
        }
        _pieces[coord] = piece;
        _pieces[storedCoord] = null;
        piece.transform.position = new Vector3(coord.Item1, coord.Item2, piece.transform.position.z);
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
