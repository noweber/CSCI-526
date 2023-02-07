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
        var lvlModel = LevelController.Instance.LevelModel;
        for (int x = 0; x < LevelController.Instance.LevelModel.GetWidth(); x++)
        {
            for (int y = 0; y < LevelController.Instance.LevelModel.GetHeight(); y++)
            {
                Tuple<int, int> position = new Tuple<int, int>(x, y);
                if (lvlModel.TryGetUnit(position) != null && !lvlModel.TryGetUnit(position).IsControlledByHuman() && !string.Equals(lvlModel.TryGetUnit(position).Name(), UnitType.Triangle))
                {
                    allEnemyPieces.Add(new Tuple<int, int>(x, y));
                }
            }
        }
    }

    public PieceController SelectRandomPiece()
    {
        GetPieces();
        var pieces = LevelController.Instance._pieces; 
        bool checkMove = true;
        while (checkMove == true)
        {
            int index = Random.Range(0, allEnemyPieces.Count);
            if (pieces.ContainsKey(allEnemyPieces[index]) && pieces[allEnemyPieces[index]] != null)
            {
                Debug.Log("ContainsKey");
                if (pieces[allEnemyPieces[index]].Name() != "Triangle")
                {
                    Debug.Log("not Triangle");
                    if (pieces[allEnemyPieces[index]].HasMoved() != true)
                    {
                        Debug.Log("hasMoved");
                        checkMove = false;
                        LevelController.Instance.storedCoord = allEnemyPieces[index];
                        pieces[allEnemyPieces[index]].SetMoveState(true);
                        return pieces[allEnemyPieces[index]];
                    }
                }
            } else
            {
                if (allEnemyPieces.Count <= 2)
                {
                    foreach(var enemy in allEnemyPieces)
                    {
                        if (pieces[enemy].Name() == "Triangle" || pieces[enemy].HasMoved() == true)
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
        var grid = LevelController.Instance;
        var piece = SelectRandomPiece();
        if (piece != null)
        {
            var moves = piece.GetLegalMoves(grid.LevelModel.GetWidth(), grid.LevelModel.GetHeight());
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
                pieces.SetMoveState(false);
            }
            StopAllCoroutines();
            GameManagerChain.Instance.NumMoves = 0;
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
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
