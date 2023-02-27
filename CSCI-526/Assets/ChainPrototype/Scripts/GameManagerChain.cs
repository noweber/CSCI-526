using Assets.Scripts.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class GameManagerChain : MonoBehaviour
{
    public static GameManagerChain Instance;

    public GameStateEnum GameStateEnum;

    /// <summary>
    /// This value trackes how many total moves have been made during the current level for analytics.
    /// </summary>
    public int TotalMoves;

    /// <summary>
    /// This field tracks the (x, y) positions of the game board which pieces move into along with a counter for analytics.
    /// This is used to produce a heat map of which parts of the board the players move into the most.
    /// </summary>
    private Dictionary<Tuple<int, int>, int> countOfTilesMovedTo;

    /// <summary>
    /// This field tracks the number of times the player moved a circle piece for end-of-level analytics.
    /// </summary>
    private int circlePiecessMovedByPlayer;

    /// <summary>
    /// This field tracks the number of times the player moved a diamond piece for end-of-level analytics.
    /// </summary>
    private int diamondPiecessMovedByPlayer;

    public bool UsedAbility = false;

    public string SceneName;

    public string playTestID;

    private float playStartTime;

    private int movesMade;

    public float movableAlpha;      // Unified alpha value used for all player pieces when game state moves to player's turn

    public bool increasingAlpha;

    public bool switchingTurns = false;        // Flag used to lock switching turns if the game is in the process of doing so

    /// <summary>
    /// This is the set of pieces which have been moved during a given turn state.
    /// </summary>
    private List<PieceMono> movedPieces;

    void Awake()
    {
        Instance = this;
        SceneName = SceneManager.GetActiveScene().name;
        movesMade = 0;
        TotalMoves = 0;
        countOfTilesMovedTo = new Dictionary<Tuple<int, int>, int>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameStateEnum.GenerateGrid);
        if (SceneName != "TutorialLevel" && SceneName != "TutorialFogOfWar")
        {
            MenuManager.Instance.ShowEndTurnButton();
        }
        playTestID = Guid.NewGuid().ToString();
        playStartTime = Time.realtimeSinceStartup;

    }
    public void LoadNextLevel(String inputLevelName)
    {
        SceneManager.LoadScene(inputLevelName);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneName);
    }
    public void ToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelection");
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
    public void AddMovedPiece(PieceMono pieceThatMoved, Tuple<int, int> destination)
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

        // Store the move made for sending the counts in analytics later:
        if (pieceThatMoved.IsHuman())
        {
            if (countOfTilesMovedTo.ContainsKey(destination))
            {
                countOfTilesMovedTo[destination]++;
            }
            else
            {
                countOfTilesMovedTo.TryAdd(destination, 1);
            }

            if (pieceThatMoved.IsCircle())
            {
                circlePiecessMovedByPlayer++;
            }

            if (pieceThatMoved.IsDiamond())
            {
                diamondPiecessMovedByPlayer++;
            }
        }
    }

    /// <summary>
    /// Resets the number of moves made this turn to 0.
    /// </summary>
    public void ResetMovesMade()
    {
        movesMade = 0;
    }

    /// <summary>
    /// Returns the number of moves made this turn.
    /// </summary>
    /// <returns>Returns an integer [0, n].</returns>
    public int GetMovesMade()
    {
        return movesMade;
    }

    /// <summary>
    /// Increases the number of moves made this turn (aka, decrease the number of moves remaining).
    /// </summary>
    /// <param name="amount">The number of moves made.</param>
    public void IncrementMoves(int amount = 1)
    {
        movesMade += amount;

        // Check if the the game is over now that number of available moves decreased during play:
        if (!LevelMono.Instance.DoEnemiesRemain())
        {
            // TODO: Transition to a win state per open tasks once designed.
            this.ChangeState(GameStateEnum.Victory);
        }
        else if (!LevelMono.Instance.DoHumansRemain())
        {
            // TODO: Transition to a lose state.
            this.ChangeState(GameStateEnum.Loss);
        }
    }

    /// <summary>
    /// Decreases the count of the number of moves made this turn.
    /// </summary>
    /// <param name="amount">The number to subtract.</param>
    public void DecrementMoves(int amount = 1)
    {
        movesMade -= amount;
    }

    private IEnumerator FadeMovableAlpha()
    {
        movableAlpha = 0.65f;
        while(true)
        {
            if (movableAlpha >= 0.65f)
            {
                increasingAlpha = false;
            }
            else if (movableAlpha <= 0.15f)
            {
                increasingAlpha = true;
            }
            if (increasingAlpha)
            {
                movableAlpha += 0.07f;
            }
            else
            {
                movableAlpha -= 0.07f;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    // TESTING using this instead of directly changing turns to briefly display an obvious turn indicator -- animate after midterm?
    public IEnumerator StateToAI()
    {
        if (!switchingTurns && GameStateEnum != GameStateEnum.Victory && GameStateEnum != GameStateEnum.Loss)     // Prevents spamming the call of this IEnumerator and bugging the turn sequence
        {
            switchingTurns = true;
            MenuManager.Instance._enemyTurnIndicator.SetActive(true);
            Debug.Log("SETTING TO AI TURN");
            yield return new WaitForSeconds(1.5f);
            MenuManager.Instance._enemyTurnIndicator.SetActive(false);

            GameManagerChain.Instance.ResetMovesMade();

            ChangeState(GameStateEnum.AI);
            switchingTurns = false;
        }

        yield return null;
    }
    public IEnumerator StateToHuman()     
    {
        if(!switchingTurns && GameStateEnum != GameStateEnum.Victory && GameStateEnum != GameStateEnum.Loss)
        {
            switchingTurns = true;
            MenuManager.Instance._playerTurnIndicator.SetActive(true);
            Debug.Log("SETTING TO PLAYER TURN");
            yield return new WaitForSeconds(1.5f);
            MenuManager.Instance._playerTurnIndicator.SetActive(false);

            GameManagerChain.Instance.ResetMovesMade();

            ChangeState(GameStateEnum.Human);
            switchingTurns = false;
        }

        yield return null;
    }

    /// <summary>
    /// Changes the current game state.
    /// </summary>
    /// <param name="newState">The game state to transition to.</param>
    public void ChangeState(GameStateEnum newState)
    {
        GameManagerChain.Instance.ClearMovedPieces();
        // GameManagerChain.Instance.ResetMovesMade();
        GameManagerChain.Instance.UsedAbility = false;

        GameStateEnum = newState;

        switch (newState)
        {
            case GameStateEnum.GenerateGrid:
                if (SceneName == "TutorialLevel")
                {
                    LevelMono.Instance.LoadLevel(TutorialLevel());
                }
                else if (SceneName == "TutorialFogOfWar")
                {
                    LevelMono.Instance.LoadLevel(TutorialFogOfWarLevel());
                }
                else
                {
                    LevelMono.Instance.LoadLevel(LevelOne());
                }
                StartCoroutine(FadeMovableAlpha());     // Start the blinking timer for movable units here
                MenuManager.Instance.SetVictoryScreen(false);
                break;
            case GameStateEnum.Human:
                MenuManager.Instance.ShowTurnInfo();
                MenuManager.Instance.ShowNumMovesInfo();
                foreach(PieceMono piece in LevelMono.Instance.GetPlayerPieces())
                {
                    if (!piece.IsTriangle())
                    {
                        piece.canMoveObject.SetActive(true);
                        piece.cantMoveObject.SetActive(false);
                    }
                }
                foreach(PieceMono piece in LevelMono.Instance.GetEnemyPieces())
                {
                    piece.canMoveObject.SetActive(false);
                    piece.canMoveObject.SetActive(false);
                }
                if (SceneName != "TutorialLevel" && SceneName != "TutorialFogOfWar")
                {
                    MenuManager.Instance.ShowEndTurnButton();
                }
                break;
            case GameStateEnum.AI:
                MenuManager.Instance.ShowTurnInfo();
                MenuManager.Instance.ShowNumMovesInfo();
                MenuManager.Instance.HideEndTurnButton();

                foreach (PieceMono piece in LevelMono.Instance.GetPlayerPieces())
                {
                    piece.canMoveObject.SetActive(false);
                    piece.cantMoveObject.SetActive(false);
                }

                if (SceneName == "TutorialLevel")
                {
                    // slacking off 
                }
                else if (SceneName == "TutorialFogOfWar")
                {
                    // slacking off
                }
                else
                {
                    EnemyAI.Instance.PerformTurn();
                }
                break;
            case GameStateEnum.Victory:
                Debug.Log("VICTORY");
                MenuManager.Instance.SetVictoryScreen(true);
                SendEndOfLevelAnalytics();
                break;
            case GameStateEnum.Loss:
                Debug.Log("LOSS");
                SendEndOfLevelAnalytics();
                MenuManager.Instance.SetDefeatScreen(true);
                break;
        }
    }

    private void SendEndOfLevelAnalytics()
    {
        float timePlayedThisLevelInSeconds = (Time.realtimeSinceStartup - playStartTime) / 60;
        string serializedMovesMadeHeat = JsonConvert.SerializeObject(countOfTilesMovedTo);
        Analytics.Instance.Send(
            playTestID,
            timePlayedThisLevelInSeconds,
            SceneName,
            LevelMono.Instance.GetWidth(),
            LevelMono.Instance.GetHeight(),
            GameManagerChain.Instance.TotalMoves,
            serializedMovesMadeHeat,
            circlePiecessMovedByPlayer,
            diamondPiecessMovedByPlayer
            );
        ResetAnalyticsCounters();
    }

    private void ResetAnalyticsCounters()
    {
        countOfTilesMovedTo = new Dictionary<Tuple<int, int>, int>();
        circlePiecessMovedByPlayer = 0;
        diamondPiecessMovedByPlayer = 0;
    }

    /// <summary>
    /// A method representing the tutorial level's data.
    /// </summary>
    /// <returns>Returns the tutorial level's data.</returns>
    private LoadLevelData TutorialLevel()
    {
        int _width = 5;
        int _height = 8;
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

    private LoadLevelData TutorialFogOfWarLevel()
    {
        int _width = 5;
        int _height = 10;       // Display the visibility of 2 triangles: 1 per team
        List<PieceInfo> units = new List<PieceInfo>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var position = new Tuple<int, int>(x, y);
                if (x == 0 && y == 0)
                {
                    units.Add(new PieceInfo(position, true, "Circle"));
                }
                if (x == 3 && y == 0)
                {
                    units.Add(new PieceInfo(position, true, "Diamond"));
                }
                if (x == 2 && y == 2)
                {
                    units.Add(new PieceInfo(position, true, "Triangle"));
                }

                // Enemies
                if (x == 4 && y == 9)
                {
                    units.Add(new PieceInfo(position, false, "Circle"));
                }
                if (x == 1 && y == 9)
                {
                    units.Add(new PieceInfo(position, false, "Diamond"));
                }
                if (x == 2 && y == 7)
                {
                    units.Add(new PieceInfo(position, false, "Triangle"));
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

                if (x == 3 && y == 3)
                {
                    units.Add(new PieceInfo(position, true, "Scout"));
                }

                if (x == 2 && y == 2 || x == _width - 3 && y == 2)
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

                if (x == 2 && y == _height - 3 || x == _width - 3 && y == _height - 3)
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

                if (x == 3 && y == 7)
                {
                    units.Add(new PieceInfo(position, false, "Scout"));
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

struct TileHeatCount
{
    public int x;
    public int y;
    public int numMovesIntoThisTIle;
}