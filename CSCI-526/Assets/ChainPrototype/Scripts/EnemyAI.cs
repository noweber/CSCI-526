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
                if (lvlModel.TryGetUnit(x, y) != null && lvlModel.TryGetUnit(x, y).Item1 != true && lvlModel.TryGetUnit(x, y).Item2 != UnitType.Triangle)
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
        while (checkMove == true)
        {
            int index = Random.Range(0, allEnemyPieces.Count);
            if (pieces.ContainsKey(allEnemyPieces[index]) && pieces[allEnemyPieces[index]] != null)
            {
                Debug.Log("ContainsKey");
                if (pieces[allEnemyPieces[index]].unitName != "Triangle")
                {
                    Debug.Log("not Triangle");
                    if (pieces[allEnemyPieces[index]].hasMoved != true)
                    {
                        Debug.Log("hasMoved");
                        checkMove = false;
                        GridManager.Instance.storedCoord = allEnemyPieces[index];
                        pieces[allEnemyPieces[index]].hasMoved = true;
                        return pieces[allEnemyPieces[index]];
                    }
                }
            } else
            {
                if (allEnemyPieces.Count <= 2)
                {
                    foreach(var enemy in allEnemyPieces)
                    {
                        if (pieces[enemy].unitName == "Triangle" || pieces[enemy].hasMoved == true)
                        {
                            checkMove = false;
                            break;
                        } 
                    }
                }
            }
            
        }
        return null;
    }

    public void MovePiece()
    {
        StartCoroutine(DelayEnemyStart());
    }

    private void PerformTurn()
    {
        var grid = GridManager.Instance;
        var piece = SelectRandomPiece();
        if (piece != null)
        {
            var moves = piece.LegalMoves(grid._width, grid._height);
            int index = Random.Range(0, moves.Count);
            if (grid.MovePiece(moves[index], piece))
            {
                GameManagerChain.Instance.MovedPieces.Add(piece);
                GameManagerChain.Instance.NumMoves += 1;
                MenuManager.Instance.ShowNumMovesInfo();
            }
        }        

        if (GameManagerChain.Instance.NumMoves == 2)
        {
            foreach (var pieces in GameManagerChain.Instance.MovedPieces)
            {
                pieces.hasMoved = false;
            }
            StopAllCoroutines();
            GameManagerChain.Instance.NumMoves = 0;
            GameManagerChain.Instance.ChangeState(GameStateEnum.White);
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
