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
    
    private List<PieceController> movedPieces = new List<PieceController>();

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
        var controller = LevelController.Instance;
        for (int x = 0; x < LevelController.Instance.LevelModel.GetWidth(); x++)
        {
            for (int y = 0; y < LevelController.Instance.LevelModel.GetHeight(); y++)
            {
                Tuple<int, int> position = new Tuple<int, int>(x, y);
                // if (lvlModel.TryGetUnit(position) != null && !lvlModel.TryGetUnit(position).IsControlledByHuman())
                // {
                //     Debug.Log(lvlModel.TryGetUnit(position).Name());
                // }
               
                if (controller.GetPiece(position) != null && !controller.GetPiece(position).IsControlledByHuman() && controller.GetPiece(position).HasMoved() == false && !string.Equals(controller.GetPiece(position).Name(), UnitType.Triangle.ToString()))
                {
                    allEnemyPieces.Add(new Tuple<int, int>(x, y));
                }
            }
        }
    }

    public PieceController SelectRandomPiece()
    {
        var controller = LevelController.Instance;
        List<PieceController> pieces = new List<PieceController>();
        
        if (allEnemyPieces.Count == 0)
        {
            return null;
        }
        
        foreach (var coord in allEnemyPieces)
        {
            if (controller.GetPiece(coord).HasMoved() == false)
            {
                pieces.Add(controller.GetPiece(coord));
            }
        }
        int index = Random.Range(0, pieces.Count);
        
        return pieces[index];
    }

    public void MovePiece()
    {
        //StartCoroutine(DelayEnemyStart());
        GetPieces();

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
            var moves = piece.GetLegalMoves(grid.LevelModel.GetWidth(), grid.LevelModel.GetHeight());
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
