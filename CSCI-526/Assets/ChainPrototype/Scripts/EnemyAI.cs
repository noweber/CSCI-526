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
        foreach (var coord in enemyPieceCoords)
        {
            if (lvlMono.GetPiece(coord).CanMove())
            {
                movableEnemies.Add(coord);
            }
        }
        if (movableEnemies.Count == 0) { return null; }
        return movableEnemies[Random.Range(0, movableEnemies.Count)];
    }

    public void PerformTurn()
    {       
        if (!isRunning)
            StartCoroutine(DelayEnemyStart());
        // PerformTurn();
        // PerformTurn();
    }

    private void MovePiece()
    {
        var lvlMono = LevelMono.Instance;
        var aiCoord = SelectRandomPiece();
        if (aiCoord != null)
        {
            var aiPiece = lvlMono.GetPiece(aiCoord);
            var moves = aiPiece.LegalMoves(lvlMono.GetWidth(), lvlMono.GetHeight());
            int index = Random.Range(0, moves.Count);
            lvlMono.SelectPiece(aiPiece, aiCoord);
            if (lvlMono.MovePiece(moves[index]))
            {
                GameManagerChain.Instance.AddMovedPiece(aiPiece);
                GameManagerChain.Instance.AddToNumberOfMovesMade(1);
                if (this.inTriangleRange(moves[index]))
                {
                    GameManagerChain.Instance.SubtractFromNumberOfMovesMade(1);
                    lvlMono.GetPiece(moves[index]).SetMoveState(false);
                }
                MenuManager.Instance.ShowNumMovesInfo();
            }
            else
            {
                lvlMono.ResetPiece();
                Debug.Log("AI FAILED TO MOVE");
            }
        }

        if (!LevelMono.Instance.DoesHumanPlayerHaveUnitsRemaining())
        {
            StopAllCoroutines();
            GameManagerChain.Instance.ChangeState(GameStateEnum.Loss);
        }
        else if (GameManagerChain.Instance.GetNumberOfMovesMade() == 2 || aiCoord == null)
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

    private bool inTriangleRange(Tuple<int, int> unitPosition)
    {
        var adjacentList = new List<Tuple<int, int>>();
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2)); //right
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2)); //left
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 + 1)); //up
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1, unitPosition.Item2 - 1)); //down
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 + 1)); //right up diag
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 + 1)); //left  up diag
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 + 1, unitPosition.Item2 - 1)); //right down diag
        adjacentList.Add(new Tuple<int, int>(unitPosition.Item1 - 1, unitPosition.Item2 - 1)); //left down diag
        var lvlMono = LevelMono.Instance;
        foreach (Tuple<int, int> coord in adjacentList)
        {
            var piece = lvlMono.GetPiece(coord);
            if (piece != null && piece.IsTriangle() && !lvlMono.GetPiece(unitPosition).IsEnemyOf(piece))
            {
                return true;
            }
        }
        return false;
    }
}
