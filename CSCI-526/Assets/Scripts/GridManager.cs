using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField] private int _width = 6, _height = 6;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Triangle _trianglePrefab;
    [SerializeField] private Diamond _diamondPrefab;
    [SerializeField] private Circle _circlePrefab;

    [SerializeField] private Transform _camera;
    
    public Dictionary<Vector2, Tile> tiles;
    private Dictionary<Vector2, Piece> _pieces;
    
    public Piece storedPiece = null;
    public Vector2 storedCoord = new Vector2(-1,-1);

    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        _pieces = new Dictionary<Vector2, Piece>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tile.Init(isOffset);

                var coord = new Vector2(x, y);
                tiles[coord] = tile;
                
                if (x == 0 && y == 0 || x == 1 && y == 0)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    triangle.isWhite = true;
                    triangle.hasMoved = false;
                    _pieces[coord] = triangle;
                   
                }
                
                if (x == 2 && y == 0 || x == 3 && y == 0)
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    circle.isWhite = true;
                    circle.hasMoved = false;
                    _pieces[coord] = circle;
                }
                
                if (x == 4 && y == 0 || x == 5 && y == 0)
                {
                    var diamond = Instantiate(_diamondPrefab, new Vector3(x, y, -1), _diamondPrefab.transform.rotation);
                    diamond.isWhite = true;
                    diamond.hasMoved = false;
                    _pieces[coord] = diamond;
                }
                
                if (x == 0 && y == _height-1 || x == 1 && y == _height-1)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    triangle.isWhite = false;
                    triangle.hasMoved = false;
                    triangle.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    _pieces[coord] = triangle;
                   
                }
                
                if (x == 2 && y == _height-1 || x == 3 && y == _height-1)
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    circle.isWhite = false;
                    circle.hasMoved = false;
                    circle.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    _pieces[coord] = circle;
                }
                
                if (x == 4 && y == _height-1 || x == 5 && y == _height-1)
                {
                    var diamond = Instantiate(_diamondPrefab, new Vector3(x, y, -1), _diamondPrefab.transform.rotation);
                    diamond.isWhite = false;
                    diamond.hasMoved = false;
                    diamond.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    _pieces[coord] = diamond;
                }
            }
        }

        _camera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.White);
    }

    public Tile GetTile(Vector2 coord)
    {
        if (tiles.TryGetValue(coord, out var tile))
        {
            return tile;
        }

        return null;
    }
    
    public Piece GetPiece(Vector2 coord)
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

    public void MovePiece(Vector2 coord, Piece piece)
    {
        _pieces[coord] = piece;
        _pieces[storedCoord] = null;

        piece.transform.position = new Vector3(coord.x, coord.y, piece.transform.position.z);
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
