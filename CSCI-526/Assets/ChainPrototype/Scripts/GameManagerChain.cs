using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerChain : MonoBehaviour
{
    public static GameManagerChain Instance;

    public GameStateEnum GameStateEnum;

    public int NumMoves;

    public bool UsedAbility = false;

    public List<Piece> MovedPieces;

    void Awake()
    {
        Instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        NumMoves = 0;
        MovedPieces = new List<Piece>();
        ChangeState(GameStateEnum.GenerateGrid);
        MenuManager.Instance.ShowEndTurnButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeState(GameStateEnum newState)
    {
        GameStateEnum = newState;
        var sceneName = SceneManager.GetActiveScene().name;
        switch (newState)
        {
            case GameStateEnum.GenerateGrid:
                if (sceneName == "TutorialLevel")
                {
                    GridManager.Instance.GenerateTutorialGrid();
                }
                else
                {
                    GridManager.Instance.GenerateGrid(); 
                }
                break;
            case GameStateEnum.White:
                break;
            case GameStateEnum.Black:
                if (sceneName == "TutorialLevel")
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
}

public enum GameStateEnum
{
    GenerateGrid = 0,
    White = 1,
    Black = 2
}