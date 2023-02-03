using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        switch (newState)
        {
            case GameStateEnum.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameStateEnum.White:
                break;
            //case GameStateEnum.Black:
            //    break;
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