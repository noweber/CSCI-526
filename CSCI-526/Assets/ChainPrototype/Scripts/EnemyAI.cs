using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;
using Assets.Scripts.Piece;
using System;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;

    private List<Tuple<int, int>> allEnemyPieces;
    
    private List<IPiece> movedPieces;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPieces()
    {
        allEnemyPieces = new List<Tuple<int, int>>();
        var lvlModel = LevelController.Instance.LevelModel;
        for (int x = 0; x < LevelController.Instance.LevelModel.GetWidth(); x++)
        {
            for (int y = 0; y < LevelController.Instance.LevelModel.GetHeight(); y++)
            {
                Tuple<int, int> position = new Tuple<int, int>(x, y);
                if (lvlModel.TryGetUnit(position) != null && !lvlModel.TryGetUnit(position).IsControlledByHuman())
                {
                    Debug.Log(lvlModel.TryGetUnit(position).Name());
                }
               
                if (lvlModel.TryGetUnit(position) != null && !lvlModel.TryGetUnit(position).IsControlledByHuman() && lvlModel.TryGetUnit(position).HasMoved() == false && !string.Equals(lvlModel.TryGetUnit(position).Name(), UnitType.Triangle.ToString()))
                {
                    allEnemyPieces.Add(new Tuple<int, int>(x, y));
                }
            }
        }
    }

    public IPiece SelectRandomPiece()
    {
        var lvlModel = LevelController.Instance.LevelModel;
        List<IPiece> pieces = new List<IPiece>();
        
        if (allEnemyPieces.Count == 0)
        {
            return null;
        }
        
        foreach (var coord in allEnemyPieces)
        {
            if (lvlModel.TryGetUnit(coord).HasMoved() == false)
            {
                pieces.Add(lvlModel.TryGetUnit(coord));
            }
        }
        int index = Random.Range(0, pieces.Count);
        
        return pieces[index];

        /*
        var pieces = LevelController.Instance._pieces;
        bool canMove = true;
        if (allEnemyPieces.Count < 2)
        {
            foreach (var enemy in allEnemyPieces)
            {
                if (pieces[enemy].HasMoved() == true)
                {
                    canMove = false;
                    return null;
                }
            }
            canMove = false;
            return null;
        }

        while (canMove == true)
        {
            int index = Random.Range(0, allEnemyPieces.Count);
            if (pieces.ContainsKey(allEnemyPieces[index]) && pieces[allEnemyPieces[index]] != null)
            {
                //Debug.Log("ContainsKey");
                if (pieces[allEnemyPieces[index]].HasMoved() != true) 
                {
                    //Debug.Log("hasMoved");
                    canMove = false;
                    LevelController.Instance.storedCoord = allEnemyPieces[index];
                    pieces[allEnemyPieces[index]].SetMoveState(true);
                    return pieces[allEnemyPieces[index]];
                }
            }
            
        }
        return null;
        */
    }

    public void MovePiece()
    {
        //StartCoroutine(DelayEnemyStart());
        GetPieces();
        var lvlModel = LevelController.Instance.LevelModel;

        if (allEnemyPieces.Count == 0)
        {
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
            return;
        }
        
        // first turn
        if (allEnemyPieces.Count == 1)
        {
            PerformTurn();
           
            //StopAllCoroutines();
            GameManagerChain.Instance.NumMoves = 0;
            foreach (var pieces in movedPieces)
            {
                pieces.SetMoveState(false);
            }
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
            return;
        }
        
        // second turn
        PerformTurn();
        PerformTurn();
        //StopAllCoroutines();
        GameManagerChain.Instance.NumMoves = 0;
        foreach (var pieces in movedPieces)
        {
            pieces.SetMoveState(false);
        }
        GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
    }

    private void PerformTurn()
    {
        var grid = LevelController.Instance;
        var piece = SelectRandomPiece();
        /*
        if (piece == null || GameManagerChain.Instance.NumMoves == 2)
        {
            foreach (var pieces in GameManagerChain.Instance.MovedPieces)
            {
                pieces.SetMoveState(false);
            }
            //StopAllCoroutines();
            GameManagerChain.Instance.NumMoves = 0;
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
            return;
        }*/

        if (piece != null)
        {
            var moves = piece.LegalMoves(grid.LevelModel.GetWidth(), grid.LevelModel.GetHeight());
            int index = Random.Range(0, moves.Count);
            if (grid.MovePiece(moves[index], piece))
            {
                movedPieces.Add(piece);
                GameManagerChain.Instance.NumMoves += 1;
                piece.SetMoveState(true);
                MenuManager.Instance.ShowNumMovesInfo();
            }
        }

    }

    private IEnumerator DelayEnemyStart()
    {
        yield return new WaitForSeconds(1f);
        PerformTurn();
        yield return new WaitForSeconds(1f);
        StartCoroutine(DelayEnemyStart());
    }
}
