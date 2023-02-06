using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;
using System;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;

    private List<Tuple<int, int>> allEnemyPieces;

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
        var lvlModel = GridManager.Instance.levelModel;
        for (int x = 0; x < GridManager.Instance._width; x++)
        {
            for (int y = 0; y < GridManager.Instance._height; y++)
            {
                if (lvlModel.TryGetUnit(x, y) != null && lvlModel.TryGetUnit(x, y).Item1 != true)
                {
                    allEnemyPieces.Add(new Tuple<int, int>(x, y));
                }
            }
        }
    }

    public Piece SelectRandomPiece()
    {
        GetPieces();
        var pieces = GridManager.Instance._pieces; 
        bool checkMove = true;
        int countFalse = 0;
        while (checkMove == true)
        {
            int index = Random.Range(0, allEnemyPieces.Count);
            if (pieces.ContainsKey(allEnemyPieces[index]) && pieces[allEnemyPieces[index]].unitName != "Triangle" && pieces[allEnemyPieces[index]].hasMoved != true)
            {
                checkMove = false;
                return pieces[allEnemyPieces[index]];
            } else
            {
                countFalse++;
            }

            if (countFalse == allEnemyPieces.Count - 2)
            {
                break;
            }
        }
        return null;
    }

    public void MovePiece()
    {
        var grid = GridManager.Instance;
        for (int i=0; i < 2; i++)
        {
            var piece = SelectRandomPiece();
            if (piece == null)
            {
                break;
            }
            var moves = piece.LegalMoves(grid._width, grid._height);
            int index = Random.Range(0, moves.Count);
            if (grid.MovePiece(moves[index], piece))
            {
                GameManagerChain.Instance.NumMoves += 1;
            }
        }

        if (GameManagerChain.Instance.NumMoves == 2)
        {
            GameManagerChain.Instance.NumMoves = 0;
            GameManagerChain.Instance.ChangeState(GameStateEnum.White);
        }

    }
}
