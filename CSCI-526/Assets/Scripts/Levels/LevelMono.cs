using Assets.Scripts.Levels;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
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

    public PieceMono storedPiece = null;
    public Tuple<int, int> storedCoord = new Tuple<int, int>(-1, -1);

    public ILevel LevelModel { get; private set; }

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

    public void LoadLevel(LoadLevelData level)
    {
        UnloadLevel();
        LevelModel = new Level(level.Width, level.Height, level.Units);
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
        tiles = new Dictionary<Tuple<int, int>, Tile>();
        _pieces = new Dictionary<Tuple<int, int>, PieceMono>();
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

                if (LevelModel.TryGetUnit(coord) != null)
                {
                    bool isHumanUnit = LevelModel.TryGetUnit(coord).IsControlledByHuman();
                    switch (Enum.Parse(typeof(UnitType), LevelModel.TryGetUnit(coord).Name()))
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

    public PieceMono GetPiece(Tuple<int, int> coord)
    {
        if (_pieces.TryGetValue(coord, out var piece))
        {
            return piece;
        }

        return null;
    }

    public LevelMono()
    {
        Instance = this;
    }

    public bool MovePiece(Tuple<int, int> coord, PieceMono piece)
    {
        var validMoves = piece.GetLegalMoves(LevelModel.GetWidth(), LevelModel.GetHeight());
        if (!validMoves.Contains(new Tuple<int, int>(coord.Item1, coord.Item2)))
        {
            return false;
        }
        if (LevelMono.Instance.GetPiece(coord) != null && string.Equals(piece.Name(), UnitType.Triangle.ToString()))
        {
            //Debug.Log("Return False Capture Triangle: " + coord);
            return false;
        }
        if (!this.LevelModel.TryMoveUnit(storedCoord, coord))
        {
            if (this.LevelModel.TryCaptureUnit(storedCoord, coord))
            {
                // TODO: Should this be destroyed here?
                Destroy(_pieces[coord].gameObject);
                _pieces.Remove(coord);

                // TODO: Don't use this direct call to update the AI's piece set:
                // EnemyAI.Instance.GetPieces();
            }
        }
        _pieces[coord] = piece;
        piece.UpdateLocation(new Vector3(coord.Item1, coord.Item2, piece.transform.position.z));
        _pieces.Remove(storedCoord);
        storedCoord = new Tuple<int, int>(-1, -1);
        return true;
    }
}
