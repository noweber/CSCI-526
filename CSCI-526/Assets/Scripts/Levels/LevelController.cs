using Assets.Scripts.Levels;
using Assets.Scripts.Piece;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; private set; }

    [SerializeField] public int _width = 6, _height = 6;

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private PieceController humanTriangleUnitPrefab;
    [SerializeField] private PieceController humanDiamondUnitPrefab;
    [SerializeField] private PieceController humanCircleUnitPrefab;
    [SerializeField] private PieceController aiTriangleUnitPrefab;
    [SerializeField] private PieceController aiDiamondUnitPrefab;
    [SerializeField] private PieceController aiCircleUnitPrefab;

    [SerializeField] private Transform _camera;

    public Dictionary<Tuple<int, int>, Tile> tiles;
    public Dictionary<Tuple<int, int>, PieceController> _pieces;

    public PieceController storedPiece = null;
    public Tuple<int, int> storedCoord = new Tuple<int, int>(-1, -1);

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
    }

    public void LoadLevel(LevelData level)
    {
        UnloadLevel();
        levelModel = new LevelModel(level.Width, level.Height, level.Units);
        CreateSceneObjects(level);
    }

    private void UnloadLevel()
    {
        // TODO: Delete all tiles
        // TODO: Delete all pieces
        // TODO: Ensure any elements of the previous level are removed from the scene.
    }

    private void CreateSceneObjects(LevelData level)
    {
        tiles = new Dictionary<Tuple<int, int>, Tile>();
        _pieces = new Dictionary<Tuple<int, int>, PieceController>();
        for (int x = 0; x < level.Width; x++)
        {
            for (int y = 0; y < level.Height; y++)
            {
                var tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                tile.Init(isOffset);

                var coord = new Tuple<int, int>(x, y);
                tiles[coord] = tile;

                if (levelModel.TryGetUnit(coord) != null)
                {
                    bool isHumanUnit = levelModel.TryGetUnit(coord).IsControlledByHuman();
                    switch (Enum.Parse(typeof(UnitType), levelModel.TryGetUnit(coord).Name()))
                    {
                        case UnitType.Circle:
                            if (isHumanUnit)
                            {
                                _pieces.Add(coord, Instantiate(humanCircleUnitPrefab, new Vector3(x, y, -1), Quaternion.identity));
                            }
                            else
                            {

                                _pieces.Add(coord, Instantiate(aiCircleUnitPrefab, new Vector3(x, y, -1), Quaternion.identity));
                            }
                            break;
                        case UnitType.Diamond:
                            if (isHumanUnit)
                            {
                                _pieces.Add(coord, Instantiate(humanDiamondUnitPrefab, new Vector3(x, y, -1), Quaternion.identity));
                            }
                            else
                            {

                                _pieces.Add(coord, Instantiate(aiDiamondUnitPrefab, new Vector3(x, y, -1), Quaternion.identity));
                            }
                            break;
                        case UnitType.Triangle:
                        default:
                            if (isHumanUnit)
                            {
                                _pieces.Add(coord, Instantiate(humanTriangleUnitPrefab, new Vector3(x, y, -1), Quaternion.identity));
                            }
                            else
                            {

                                _pieces.Add(coord, Instantiate(aiTriangleUnitPrefab, new Vector3(x, y, -1), Quaternion.identity));
                            }
                            break;
                    }
                }
            }
        }

        _camera.transform.position = new Vector3((float)level.Width / 2 - 0.5f, (float)level.Height / 2 - 0.5f, -10);
        GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
    }

    public Tile GetTile(Tuple<int, int> coord)
    {
        if (tiles.TryGetValue(coord, out var tile))
        {
            return tile;
        }

        return null;
    }

    public PieceController GetPiece(Tuple<int, int> coord)
    {
        if (_pieces.TryGetValue(coord, out var piece))
        {
            return piece;
        }

        return null;
    }

    public LevelController()
    {
        Instance = this;
    }

    public bool MovePiece(Tuple<int, int> coord, PieceController piece)
    {
        var validMoves = piece.GetLegalMoves(levelModel.GetWidth(), levelModel.GetHeight());
        if (!validMoves.Contains(new Tuple<int, int>(coord.Item1, coord.Item2)))
        {
            return false;
        }
        if (LevelController.Instance.GetPiece(coord) != null && string.Equals(piece.Name(), UnitType.Triangle.ToString()))
        {
            //Debug.Log("Return False Capture Triangle: " + coord);
            return false;
        }
        this.levelModel.TryMoveUnit(storedCoord, coord);
        _pieces[coord] = piece;
        _pieces[storedCoord] = null;
        piece.UpdateLocation(new Vector3(coord.Item1, coord.Item2, piece.transform.position.z));
        storedCoord = new Tuple<int, int>(-1, -1);
        return true;
    }
}
