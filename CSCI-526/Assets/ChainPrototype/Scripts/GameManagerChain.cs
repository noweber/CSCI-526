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

    public int TotalMoves;

    public bool UsedAbility = false;

    public string SceneName;

    public string playTestID;
    private float playStartTime;

    private int numberOfMovesMade;

    /// <summary>
    /// This is the set of pieces which have been moved during a given turn state.
    /// </summary>
    private List<PieceMono> movedPieces;

    void Awake()
    {
        Instance = this;
        SceneName = SceneManager.GetActiveScene().name;

    }

    // Start is called before the first frame update
    void Start()
    {
        numberOfMovesMade = 0;
        TotalMoves = 0;
        ChangeState(GameStateEnum.GenerateGrid);
        MenuManager.Instance.ShowEndTurnButton();
        playTestID = Guid.NewGuid().ToString();
        playStartTime = Time.realtimeSinceStartup;

    }

    /// <summary>
    /// This method resets the move state of the list of any previously moved units and then clears the list.
    /// </summary>
    public void ClearMovedPieces()
    {
        SetAllMovedPiecesMoveState(false);
        movedPieces = new List<PieceMono>();
    }

    /// <summary>
    /// Adds a piece to the list of moved units this turn.
    /// </summary>
    /// <param name="pieceThatMoved">A reference to the piece that made their move already.</param>
    public void AddMovedPiece(PieceMono pieceThatMoved)
    {
        // This assumes there is no need to ever try to move a null piece.
        if (pieceThatMoved == null)
        {
            return;
        }

        // This ensures the list will always exist when called:
        if (movedPieces == null)
        {
            ClearMovedPieces();
        }

        movedPieces.Add(pieceThatMoved);
    }

    /// <summary>
    /// Resets the number of moves made this turn to 0.
    /// </summary>
    public void ResetNumberOfMovesMade()
    {
        numberOfMovesMade = 0;
    }

    /// <summary>
    /// Returns the number of moves made this turn.
    /// </summary>
    /// <returns>Returns an integer [0, n].</returns>
    public int GetNumberOfMovesMade()
    {
        return numberOfMovesMade;
    }

    /// <summary>
    /// Increases the number of moves made this turn (aka, decrease the number of moves remainig).
    /// </summary>
    /// <param name="amount">The number of moves made.</param>
    public void AddToNumberOfMovesMade(int amount = 1)
    {
        // Check if the the game is over now that number of available moves decreased during play:
        if (!LevelMono.Instance.DoesAiPlayerHaveUnitsRemaining())
        {
            // TODO: Transition to a win state per open tasks once designed.
        }
        else if (!LevelMono.Instance.DoesHumanPlayerHaveUnitsRemaining())
        {
            // TODO: Transition to a lose state.
        }
        numberOfMovesMade += amount;
    }

    /// <summary>
    /// Decreases the count of the number of moves made this turn.
    /// </summary>
    /// <param name="amount">The number to subtract.</param>
    public void SubtractFromNumberOfMovesMade(int amount = 1)
    {
        numberOfMovesMade -= amount;
    }

    /// <summary>
    /// Changes the current game state.
    /// </summary>
    /// <param name="newState">The game state to transition to.</param>
    public void ChangeState(GameStateEnum newState)
    {
        GameManagerChain.Instance.ClearMovedPieces();
        GameManagerChain.Instance.ResetNumberOfMovesMade();
        GameManagerChain.Instance.UsedAbility = false;

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
                MenuManager.Instance.ShowTurnInfo();
                MenuManager.Instance.ShowEndTurnButton();
                break;
            case GameStateEnum.AI:
                MenuManager.Instance.ShowTurnInfo();
                MenuManager.Instance.HideEndTurnButton();
                if (SceneName == "TutorialLevel")
                {
                    // slacking off 
                }
                else
                {
                    MenuManager.Instance.ShowTurnInfo();
                    MenuManager.Instance.HideEndTurnButton();
                    EnemyAI.Instance.PerformTurn();
                }
                break;
            case GameStateEnum.Victory:
            case GameStateEnum.Loss:
                // TODO: Add victory and loss game state logic. This currently just resets the game.
                Scene scene = SceneManager.GetActiveScene();
                
                float time_level = (Time.realtimeSinceStartup - playStartTime)/60 ;
                //Debug.Log("time"+  time_level);
                Analytics.Instance.Send(playTestID, GameManagerChain.Instance.TotalMoves, scene.name , time_level );
                SceneManager.LoadScene(scene.name);
                break;
        }
        MenuManager.Instance.ShowTurnInfo();
        MenuManager.Instance.ShowNumMovesInfo();

    }

    /// <summary>
    /// A method representing the tutorial level's data.
    /// </summary>
    /// <returns>Returns the tutorial level's data.</returns>
    private LoadLevelData TutorialLevel()
    {
        int _width = 5;
        int _height = 13;
        List<PieceInfo> units = new List<PieceInfo>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var position = new Tuple<int, int>(x, y);
                if (x == 2 && y == 3)
                {
                    units.Add(new PieceInfo(position, true, "Triangle"));
                }

                if (x == 0 && y == 0)
                {
                    units.Add(new PieceInfo(position, true, "Circle"));
                }

                if (x == 3 && y == 0)
                {
                    units.Add(new PieceInfo(position, true, "Diamond"));
                }

                // enemies
                if (x == 3 && y == _height - 4 || x == 3 && y == _height - 3)
                {
                    units.Add(new PieceInfo(position, false, "Circle"));
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

    /// <summary>
    /// A method representing the first level's data.
    /// </summary>
    /// <returns>Returns the first level's data.</returns>
    private LoadLevelData LevelOne()
    {
        int _width = 8;
        int _height = 10;
        List<PieceInfo> units = new List<PieceInfo>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tuple<int, int> position = new(x, y);
                if (x == 2 && y == 3 || x == _width - 3 && y == 3)
                {
                    units.Add(new PieceInfo(position, true, "Triangle"));
                }

                if (x == 0 && y == 0 || x == _width - 1 && y == 0)
                {
                    units.Add(new PieceInfo(position, true, "Circle"));
                }

                if (x == 3 && y == 0 || x == 4 && y == 0)
                {
                    units.Add(new PieceInfo(position, true, "Diamond"));
                }

                if (x == 2 && y == _height - 4 || x == _width - 3 && y == _height - 4)
                {
                    units.Add(new PieceInfo(position, false, "Triangle"));

                }

                if (x == 0 && y == _height - 1 || x == _width - 1 && y == _height - 1)
                {
                    units.Add(new PieceInfo(position, false, "Circle"));
                }

                if (x == 3 && y == _height - 1 || x == 4 && y == _height - 1)
                {
                    units.Add(new PieceInfo(position, false, "Diamond"));
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

    /// <summary>
    /// This method sets/resets all of the moved pieces move state.
    /// </summary>
    /// <param name="state">True or false for whether the unit can still move or not.</param>
    private void SetAllMovedPiecesMoveState(bool state)
    {
        if (movedPieces == null || movedPieces.Count == 0)
        {
            return;
        }

        foreach (var piece in movedPieces)
        {
            piece.SetMoveState(state);
        }
    }
}

public enum GameStateEnum
{
    GenerateGrid = 0,
    Human = 1,
    AI = 2,
    Victory = 3,
    Loss = 4
}