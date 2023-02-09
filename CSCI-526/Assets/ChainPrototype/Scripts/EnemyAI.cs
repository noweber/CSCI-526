using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;
using System;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;

    void Awake()
    {
        Instance = this;
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
                GameManagerChain.Instance.MovedPieces.Add(aiPiece);
                GameManagerChain.Instance.NumMoves += 1;
                MenuManager.Instance.ShowNumMovesInfo();
            }
            else
            {
                lvlMono.ResetPiece();
                Debug.Log("AI FAILED TO MOVE");
            }
        }

        if (GameManagerChain.Instance.NumMoves == 2 || aiCoord == null)
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
        MovePiece();
        yield return new WaitForSeconds(1f);
        StartCoroutine(DelayEnemyStart());
    }
}
