using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState GameState;

    public int NumMoves;

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
        ChangeState(GameState.GenerateGrid);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeState(GameState newState)
    {
        GameState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.White:
                break;
            //case GameState.Black:
            //    break;
            //default:
            //   throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    White = 1,
    Black = 2
}