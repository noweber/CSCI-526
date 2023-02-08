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

    public List<PieceMono> MovedPieces;
    public string SceneName;


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
        MovedPieces = new List<PieceMono>();

        TotalMoves = 0;
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
                    LevelMono.Instance.LoadLevel(TutorialLevel());
                }
                else
                {
                    LevelMono.Instance.LoadLevel(LevelOne());
                }
                break;
            case GameStateEnum.Human:
                MenuManager.Instance.ShowEndTurnButton();
                break;
            case GameStateEnum.AI:
                if (SceneName == "TutorialLevel")
                {
                    // slacking off 
                }
                else
                {
                    MenuManager.Instance.HideEndTurnButton();
                    EnemyAI.Instance.MovePiece();
                }
                break;
                //default:
                //   throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        MenuManager.Instance.ShowTurnInfo();
        MenuManager.Instance.ShowNumMovesInfo();

    }

    private LoadLevelData TutorialLevel()
    {
        int _width = 5;
        int _height = 13;
        Dictionary<Tuple<int, int>, IPiece> units = new();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var position = new Tuple<int, int>(x, y);
                if (x == 2 && y == 3)
                {
                    units.Add(position, new Triangle(position, true));

                }

                if (x == 0 && y == 0)
                {
                    units.Add(position, new Circle(position, true));
                }

                if (x == 3 && y == 0)
                {
                    units.Add(position, new Diamond(position, true));
                }

                // enemies
                if (x == 3 && y == _height - 4 || x == 3 && y == _height - 3)
                {
                    units.Add(position, new Circle(position, false));
                }
            }
        }
        return new LoadLevelData()
        {
            Width = _width,
            Height = _height,
            Units = units
        };
    }

    private LoadLevelData LevelOne()
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
                    units.Add(position, new Triangle(position, true));
                }

                if (x == 0 && y == 0 || x == _width - 1 && y == 0)
                {
                    units.Add(position, new Circle(position, true));
                }

                if (x == 3 && y == 0 || x == 4 && y == 0)
                {
                    units.Add(position, new Diamond(position, true));
                }

                if (x == 2 && y == _height - 4 || x == _width - 3 && y == _height - 4)
                {
                    units.Add(position, new Triangle(position, false));

                }

                if (x == 0 && y == _height - 1 || x == _width - 1 && y == _height - 1)
                {
                    units.Add(position, new Circle(position, false));
                }

                if (x == 3 && y == _height - 1 || x == 4 && y == _height - 1)
                {
                    units.Add(position, new Diamond(position, false));
                }
            }
        }
        return new LoadLevelData()
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