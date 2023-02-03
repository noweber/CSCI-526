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
    
    public Dictionary<Vector2, Tile> tiles;
    public Dictionary<Vector2, Piece> _pieces;
    
    public Piece storedPiece = null;
    public Vector2 storedCoord = new Vector2(-1,-1);
    //public List<Vector2> storedValidMoves = storedPiece.LegalMoves(_width, _height) ? null;

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
                
                if (x == 2 && y == 3 || x == _width-3 && y == 3)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    triangle.isWhite = true;
                    triangle.hasMoved = false;
                    triangle.gameObject.GetComponent<SpriteRenderer>().color = colorOne;
                    _pieces[coord] = triangle;
                   
                }
                
                if (x == 0 && y == 0 || x == _width-1 && y == 0)
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

                if (x == 2 && y == _height-4 || x == _width-3 && y == _height-4)
                {
                    var triangle = Instantiate(_trianglePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    triangle.isWhite = false;
                    triangle.hasMoved = false;
                    triangle.gameObject.GetComponent<SpriteRenderer>().color = colorTwo;
                    _pieces[coord] = triangle;
                   
                }
                
                if (x == 0 && y == _height-1 || x == _width-1 && y == _height-1)
                {
                    var circle = Instantiate(_circlePrefab, new Vector3(x, y, -1), Quaternion.identity);
                    circle.isWhite = false;
                    circle.hasMoved = false;
                    circle.gameObject.GetComponent<SpriteRenderer>().color = colorTwo;
                    _pieces[coord] = circle;
                }
                
                if (x == 3 && y == _height-1 || x == 4 && y == _height-1)
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

    public bool MovePiece(Vector2 coord, Piece piece)
    {
        var validMoves = piece.LegalMoves(_width, _height);
        if (!validMoves.Contains(coord))
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
        piece.transform.position = new Vector3(coord.x, coord.y, piece.transform.position.z);
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