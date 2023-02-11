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
