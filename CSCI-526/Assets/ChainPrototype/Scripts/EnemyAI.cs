using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;
using System;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;
    private bool isRunning;

    void Awake()
    {
        Instance = this;
        isRunning = false;
    }

    public Tuple<int, int> SelectRandomPiece()
    {
        var lvlMono = LevelMono.Instance;
        var enemyPieceCoords = lvlMono.GetEnemyPieceCoords();
        if (enemyPieceCoords.Count == 0)
        {
            return null;
        }

        List<Tuple<int, int>> movableEnemies = new List<Tuple<int, int>>();
        List<Tuple<int, int>> movableDiamonds = new List<Tuple<int, int>>();
        List<Tuple<int, int>> movableCircles = new List<Tuple<int, int>>();
        foreach (var coord in enemyPieceCoords)
        {
            if (lvlMono.GetPiece(coord).CanMove())
            {
                movableEnemies.Add(coord);
                if (lvlMono.GetPiece(coord).IsDiamond())
                {
                    movableDiamonds.Add(coord);
                }
                else if (lvlMono.GetPiece(coord).IsCircle())
                {
                    movableCircles.Add(coord);
                }
            }
        }
        if (movableEnemies.Count == 0) { return null; }

        foreach (var circle in movableCircles)
        {
            if (ShouldMoveCircle(circle))
            {
                return circle;
            }
        }

        if (GameManagerChain.Instance.GetMovesMade() == 0)
        {
            foreach (var diamond in movableDiamonds)
            {
                if (ShouldMoveDiamondToCircle(diamond))
                {
                    return diamond;
                }
            }
        }

        return movableEnemies[Random.Range(0, movableEnemies.Count)];
    }

    private bool ShouldMoveDiamondToCircle(Tuple<int, int> diamondCoord)
    {
        var lvlMono = LevelMono.Instance;
        if (lvlMono.GetPiece(diamondCoord).IsAdjacentToAllyCircle())
        {
            return false;
        }
        else if (GetCircles().Count == 0)
        {
            return false;
        }
        return true;
    }

    private bool ShouldMoveCircle(Tuple<int, int> circleCoord)
    {
        var lvlMono = LevelMono.Instance;
        if (lvlMono.GetPiece(circleCoord).IsAdjacentToAllyDiamond())
        {
            return true;
        }

        //TODO: add clause to check if circle is close to enemy - move if so
        

        return false;
    }

    private List<Tuple<int, int>> GetCircles()
    {
        List<Tuple<int, int>> circles = new List<Tuple<int, int>>();
        var enemyPieceCoords = LevelMono.Instance.GetEnemyPieceCoords();
        foreach (var piece in enemyPieceCoords)
        {
            if (LevelMono.Instance.GetPiece(piece).IsCircle())
            {
                circles.Add(piece);
            }
        }
        return circles;
    }

    private Tuple<int,int> MoveDiamondToCircle(Tuple<int, int> diamond, List<Tuple<int, int>> moves)
    {
        var circles = GetCircles();
        int index = Random.Range(0, moves.Count);
        int minDistance = LevelMono.Instance.GetHeight()*LevelMono.Instance.GetWidth();
        for (int i = 0; i < moves.Count; i++)
        {
            foreach (var circle in circles)
            {
                var distance = CalculateDistance(circle, moves[i]);
                if (distance < minDistance)
                {
                    index = i;
                    minDistance = distance;
                }
            }
        }

        return moves[index];
    }

    private int CalculateDistance(Tuple<int, int> start, Tuple<int, int> end)
    {
        return Math.Abs(start.Item1 - end.Item1) + Math.Abs(start.Item2 - end.Item2);
    }

    public void PerformTurn()
    {       
        if (!isRunning)
            StartCoroutine(DelayEnemyStart());
        // PerformTurn();
        // PerformTurn();
    }

    private bool IsACapturingMove(Tuple<int, int> destination)
    {
        //TODO: account for enemy visibility
        var pieceAtDestination = LevelMono.Instance.GetPiece(destination);

        //If piece at destination not null and is Player and AI has visibility
        if (pieceAtDestination != null && pieceAtDestination.IsHuman() && LevelMono.Instance.tiles[destination].GetVisibility() == VisibilityState.Enemy)
        {
            return true;
        }
        return false;
    }

    private int PickBestMove(List<Tuple<int, int>> moves)
    {
        int bestMove = -1;
        int minDistance = LevelMono.Instance.GetHeight() * LevelMono.Instance.GetWidth();
        for (int i=0; i<moves.Count; i++)
        {
            //If AI has visibility AND the move is a capturing move
            if (LevelMono.Instance.tiles[moves[i]].GetVisibility() == VisibilityState.Enemy && IsACapturingMove(moves[i]))
                bestMove = i;
        }

        if (bestMove == -1)
        {
            for (int i = 0; i < moves.Count; i++)
            {
                foreach (var tower in LevelMono.Instance.towerLocations)
                {
                    if (LevelMono.Instance.GetPiece(tower).IsHuman())
                    {
                        var distance = CalculateDistance(tower, moves[i]);
                        if (distance < minDistance)
                        {
                            bestMove = i;
                            minDistance = distance;
                        }
                    }
                }
            }
        }

        if (bestMove == -1)
        {
            bestMove = Random.Range(0, moves.Count);
        }
        return bestMove;
    }



    //Ranking Movement:
    //1. Diamond to Circle synergy
    //2. "PickBestMove" --> prioritize captures
    //3. move to Enemy Towers
    private void MovePiece()
    {
        var lvlMono = LevelMono.Instance;
        var aiCoord = SelectRandomPiece();
        if (aiCoord != null)
        {
            var aiPiece = lvlMono.GetPiece(aiCoord);
            lvlMono.SelectPiece(aiPiece, aiCoord);
            var moves = aiPiece.LegalMoves(lvlMono.GetWidth(), lvlMono.GetHeight());
            Tuple<int, int> destination = moves[Random.Range(0, moves.Count)];

            if (aiPiece.IsDiamond() && ShouldMoveDiamondToCircle(aiCoord))
            {
                destination = MoveDiamondToCircle(aiCoord, moves);
            }
            else
            {
                int index = PickBestMove(moves);
                destination = moves[index];
            }

            if (lvlMono.MovePiece(destination))
            {
                GameManagerChain.Instance.AddMovedPiece(aiPiece);
                GameManagerChain.Instance.IncrementMoves(1);
                MenuManager.Instance.ShowNumMovesInfo();
            }
            else
            {
                lvlMono.ResetPiece();
                Debug.Log("AI FAILED TO MOVE");
                
            }
        }

/*
        if (!LevelMono.Instance.DoHumansRemain())
        {
            StopAllCoroutines();
            GameManagerChain.Instance.ChangeState(GameStateEnum.Loss);
        }
*/
        if (GameManagerChain.Instance.GetMovesMade() == 2 || aiCoord == null)
        {
            StopAllCoroutines();
            isRunning = false;
            GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        }
    }

    private IEnumerator DelayEnemyStart()
    {
        isRunning = true;
        yield return new WaitForSeconds(1f);
        MovePiece();
        yield return new WaitForSeconds(1f);
        StartCoroutine(DelayEnemyStart());
    }
}
