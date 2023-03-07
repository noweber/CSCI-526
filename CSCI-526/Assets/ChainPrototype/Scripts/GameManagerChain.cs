using Assets.Scripts.Levels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Assets.Scripts.Analytics;

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
    private Dictionary<Tuple<int, int>, int> tilesMoveToHeatmap;

    /// <summary>
    /// This field tracks the (x, y) positions of the game board where pieces are with a counter for analytics.
    /// This is used to produce a heat map of which parts of the board pieces are located whether idle/defending or not.
    /// </summary>
    private Dictionary<Tuple<int, int>, int> tilesOccupiedHeatmap;

    /// <summary>
    /// This field tracks number of times each type of piece is moved.
    /// The key is a string of the piece name (Circle, Diamond, etc.) and the value is an integer of the number of times the human player moved said piece.
    /// </summary>
    private Dictionary<string, int> humanPlayerPieceTypeMoveCounts;

    /// <summary>
    /// This field tracks the number of times the player moved a circle piece for end-of-level analytics.
    /// </summary>
    private int circlePiecessMovedByPlayer;

    /// <summary>
    /// This field tracks the number of times the player moved a diamond piece for end-of-level analytics.
    /// </summary>
    private int diamondPiecessMovedByPlayer;

    /// <summary>
    /// This field tracks the number of times the player moved a diamond piece for end-of-level analytics.
    /// </summary>
    private int scoutPiecessMovedByPlayer;

    public bool UsedAbility = false;

    public string SceneName;

    public string playTestID;

    private float playStartTime;

    private int movesMade;

    public float movableAlpha;      // Unified alpha value used for all player pieces when game state moves to player's turn

    public bool increasingAlpha;

    public bool switchingTurns = false;        // Flag used to lock switching turns if the game is in the process of doing so

    public LevelReplayData replayDataForAnalytics;

    /// <summary>
    /// This is the set of pieces which have been moved during a given turn state.
    /// </summary>
    private List<PieceMono> movedPieces;

    private void AddPieceToInitialLevelStateForReplayAnalytics(bool isHumanOwnedPiece, string pieceType, int xPosition, int yPostiion)
    {
        replayDataForAnalytics.InitialPiecePositions.Add(new PieceData()
        {
            Player = GetPlayerName(isHumanOwnedPiece),
            Piece = pieceType,
            Position = new Tuple<int, int>(xPosition, yPostiion)
        });
    }

    public void AddPieceMovementForReplayAnalytics(bool isHumanOwnedPiece, string pieceType, int xPositionStart, int yPositionStart, int xPositionDestination, int yPositionDestination)
    {
        replayDataForAnalytics.MovesMade.Add(new MoveData()
        {
            Player = GetPlayerName(isHumanOwnedPiece),
            Piece = pieceType,
            Position = new Tuple<int, int>(xPositionStart, yPositionStart),
            Destination = new Tuple<int, int>(xPositionDestination, yPositionDestination)
        });
    }

    private string GetPlayerName(bool isHuman)
    {
        if (isHuman)
        {
            return "Human";
        }
        return "AI";
    }

    void Awake()
    {
        Instance = this;
        SceneName = SceneManager.GetActiveScene().name;
        movesMade = 0;
        TotalMoves = 0;
        tilesMoveToHeatmap = new Dictionary<Tuple<int, int>, int>();
        tilesOccupiedHeatmap = new Dictionary<Tuple<int, int>, int>();
        ResetPieceMovementCountsForHumanPlayer();
        replayDataForAnalytics = new LevelReplayData();
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
        Analytics.Instance.SendStartOfLevelData(
            playTestID,
            SceneName,
            LevelMono.Instance.GetWidth(),
            LevelMono.Instance.GetHeight());
        playStartTime = Time.realtimeSinceStartup;

        // Store the initial positions for all pieces to send as analytics data later:
        foreach (var pieceEntry in LevelMono.Instance._pieces)
        {
            AddPieceToInitialLevelStateForReplayAnalytics(pieceEntry.Value.IsHuman(), pieceEntry.Value.UnitName, pieceEntry.Key.Item1, pieceEntry.Key.Item2);
        }
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
            if (tilesMoveToHeatmap.ContainsKey(destination))
            {
                tilesMoveToHeatmap[destination]++;
            }
            else
            {
                tilesMoveToHeatmap.TryAdd(destination, 1);
            }

            // TODO: After Week 10: Refactor and remove the individual counters and only use the dictionary
            if (pieceThatMoved.IsCircle())
            {
                circlePiecessMovedByPlayer++;
            }

            if (pieceThatMoved.IsDiamond())
            {
                diamondPiecessMovedByPlayer++;
            }

            if (pieceThatMoved.IsScout())
            {
                scoutPiecessMovedByPlayer++;
            }

            humanPlayerPieceTypeMoveCounts[pieceThatMoved.UnitName]++;
            //AddPieceMovementForReplayAnalytics(pieceThatMoved.IsHuman(), pieceThatMoved.UnitName, (int)pieceThatMoved.transform.position.x, (int)pieceThatMoved.transform.position.y, destination.Item1, destination.Item2);
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
        if (SceneName == "Challenge_Circle")
        {
            Debug.Log("TOTAL MOVES: " + TotalMoves);
            if (TotalMoves == 4)
            {
                if (!LevelMono.Instance.DoEnemiesRemain()) { this.ChangeState(GameStateEnum.Victory); }
                else { this.ChangeState(GameStateEnum.Loss); }
            }
        }
        else
        {
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
        while (true)
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
        if (!switchingTurns && GameStateEnum != GameStateEnum.Victory && GameStateEnum != GameStateEnum.Loss)
        {
            switchingTurns = true;
            MenuManager.Instance._playerTurnIndicator.SetActive(true);
            Debug.Log("SETTING TO PLAYER TURN");
            yield return new WaitForSeconds(1.5f);
            MenuManager.Instance._playerTurnIndicator.SetActive(false);

            GameManagerChain.Instance.ResetMovesMade();

            // This adds all of the active pieces in the level into a heatmap each time there is a transition into the human player's turn:
            foreach (var piece in LevelMono.Instance._pieces)
            {
                if (tilesOccupiedHeatmap.ContainsKey(piece.Key))
                {
                    tilesOccupiedHeatmap[piece.Key]++;
                }
                else
                {
                    tilesOccupiedHeatmap.TryAdd(piece.Key, 1);
                }
            }

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
                    LevelMono.Instance.LoadLevel(Levels.TutorialLevel());
                }
                else if (SceneName == "TutorialFogOfWar")
                {
                    LevelMono.Instance.LoadLevel(Levels.TutorialFogOfWarLevel());
                }
                else if (SceneName == "Level_One")
                {
                    LevelMono.Instance.LoadLevel(Levels.LevelOne());
                }
                else if (SceneName == "Level_Two")
                {
                    LevelMono.Instance.LoadLevel(Levels.LevelTwo());
                }
                else if (SceneName == "Challenge_Circle")
                {
                    LevelMono.Instance.LoadLevel(Levels.ChallengeCircle());
                }
                else if (SceneName == "Challenge_Scout")
                {
                    LevelMono.Instance.LoadLevel(Levels.ChallengeScout());
                }
                StartCoroutine(FadeMovableAlpha());     // Start the blinking timer for movable units here
                MenuManager.Instance.SetVictoryScreen(false);
                break;
            case GameStateEnum.Human:
                MenuManager.Instance.ShowTurnInfo();
                MenuManager.Instance.ShowNumMovesInfo();
                foreach (PieceMono piece in LevelMono.Instance.GetPlayerPieces())
                {
                    if (!piece.IsTriangle())
                    {
                        piece.canMoveObject.SetActive(true);
                        piece.cantMoveObject.SetActive(false);
                    }
                }
                foreach (PieceMono piece in LevelMono.Instance.GetEnemyPieces())
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

                if (SceneName == "TutorialLevel" || SceneName == "Challenge_Circle")
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
                SendEndOfLevelAnalytics(true);
                break;
            case GameStateEnum.Loss:
                Debug.Log("LOSS");
                SendEndOfLevelAnalytics(false);
                MenuManager.Instance.SetDefeatScreen(true);
                break;
        }
    }

    private void SendEndOfLevelAnalytics(bool humanPlayerWasVictorious)
    {
        float timePlayedThisLevelInSeconds = (Time.realtimeSinceStartup - playStartTime) / 60;
        string serializedMovesMadeHeatmapData = JsonConvert.SerializeObject(tilesMoveToHeatmap);
        string serializedTilesOccupiedHeatmapData = JsonConvert.SerializeObject(tilesOccupiedHeatmap);
        string serializedPiecesMovedByHumanPlayerCountData = JsonConvert.SerializeObject(humanPlayerPieceTypeMoveCounts);
        replayDataForAnalytics.VictoriousPlayerName = GetPlayerName(humanPlayerWasVictorious);
        string serializedReplayData = JsonConvert.SerializeObject(replayDataForAnalytics);
        Analytics.Instance.SendEndOfLevelData(
            playTestID,
            timePlayedThisLevelInSeconds,
            SceneName,
            LevelMono.Instance.GetWidth(),
            LevelMono.Instance.GetHeight(),
            GameManagerChain.Instance.TotalMoves,
            serializedTilesOccupiedHeatmapData,
            serializedMovesMadeHeatmapData,
            circlePiecessMovedByPlayer,
            diamondPiecessMovedByPlayer,
            scoutPiecessMovedByPlayer,
            serializedPiecesMovedByHumanPlayerCountData,
            serializedReplayData
            );
        ResetAnalyticsCounters();
    }

    // TODO: Week 10 - move the analytics counters to the analytics service.
    private void ResetAnalyticsCounters()
    {
        tilesMoveToHeatmap = new Dictionary<Tuple<int, int>, int>();
        tilesOccupiedHeatmap = new Dictionary<Tuple<int, int>, int>();
        ResetPieceMovementCountsForHumanPlayer();
        circlePiecessMovedByPlayer = 0;
        diamondPiecessMovedByPlayer = 0;
        scoutPiecessMovedByPlayer = 0;
        replayDataForAnalytics = new LevelReplayData();
    }

    private void ResetPieceMovementCountsForHumanPlayer()
    {
        // TODO: Make a function to increment the counts which checks for these keys
        humanPlayerPieceTypeMoveCounts = new Dictionary<string, int>();
        humanPlayerPieceTypeMoveCounts.Add(PieceMono.Circle, 0);
        humanPlayerPieceTypeMoveCounts.Add(PieceMono.Diamond, 0);
        humanPlayerPieceTypeMoveCounts.Add(PieceMono.Triangle, 0);
        humanPlayerPieceTypeMoveCounts.Add(PieceMono.Scout, 0);
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