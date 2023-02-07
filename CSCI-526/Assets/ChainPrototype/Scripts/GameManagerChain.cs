using Assets.Scripts.Levels;
using Assets.Scripts.Piece;
using Assets.Scripts.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerChain : MonoBehaviour
{
    public static GameManagerChain Instance;

    public GameStateEnum GameStateEnum;

    public int NumMoves;

    public int TotalMoves;

    public bool UsedAbility = false;

    public List<PieceController> MovedPieces;
    public string SceneName;

    public List<Piece> MovedPieces;

    public string playTestID;

    void Awake()
    {
        Instance = this;
        SceneName = SceneManager.GetActiveScene().name;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        NumMoves = 0;
        MovedPieces = new List<PieceController>();

        TotalMoves = 0;
        MovedPieces = new List<Piece>();
        ChangeState(GameStateEnum.GenerateGrid);
        MenuManager.Instance.ShowEndTurnButton();

        playTestID = System.Guid.NewGuid().ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeState(GameStateEnum newState)
    {
        GameStateEnum = newState;
        
        switch (newState)
        {
            case GameStateEnum.GenerateGrid:
                if (SceneName == "TutorialLevel")
                {
                    GridManager.Instance.GenerateTutorialGrid();
                }
                else
                {
                    LevelController.Instance.LoadLevel(LevelOne());
                }
                break;
            case GameStateEnum.Human:
                break;
            case GameStateEnum.AI:
            case GameStateEnum.Black:
                if (SceneName == "TutorialLevel")
                {
                    // slacking off 
                }
                else
                {
                    EnemyAI.Instance.MovePiece();
                }
                break;
            //default:
            //   throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        MenuManager.Instance.ShowTurnInfo();
        MenuManager.Instance.ShowNumMovesInfo();

    }

    private LevelData LevelOne()
    {
        int _width = 8;
        int _height = 10;
        Dictionary<Tuple<int, int>, IPiece> units = new();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tuple<int, int> position = new(x, y);
                if (x == 2 && y == 3 || x == _width - 3 && y == 3)
                {
                    units.Add(position, new TriangleModel(position, true));
                }

                if (x == 0 && y == 0 || x == _width - 1 && y == 0)
                {
                    units.Add(position, new CircleModel(position, true));
                }

                if (x == 3 && y == 0 || x == 4 && y == 0)
                {
                    units.Add(position, new DiamondModel(position, true));
                }

                if (x == 2 && y == _height - 4 || x == _width - 3 && y == _height - 4)
                {
                    units.Add(position, new TriangleModel(position, false));

                }

                if (x == 0 && y == _height - 1 || x == _width - 1 && y == _height - 1)
                {
                    units.Add(position, new CircleModel(position, false));
                }

                if (x == 3 && y == _height - 1 || x == 4 && y == _height - 1)
                {
                    units.Add(position, new DiamondModel(position, false));
                }
            }
        }
        return new LevelData()
        {
            Width = _width,
            Height = _height,
            Units = units
        };
    }
}

public enum GameStateEnum
{
    GenerateGrid = 0,
    Human = 1,
    AI = 2
}