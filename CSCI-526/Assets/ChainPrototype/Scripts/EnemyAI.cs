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

    private List<Tuple<int, int>> GetPieces()
    {
        List<Tuple<int, int>> allEnemyPieces = new();
        var lvlModel = LevelMono.Instance.LevelModel;
        for (int x = 0; x < LevelMono.Instance.LevelModel.GetWidth(); x++)
        {
            for (int y = 0; y < LevelMono.Instance.LevelModel.GetHeight(); y++)
            {
                Tuple<int, int> position = new(x, y);
                if (lvlModel.TryGetUnit(position) != null && !lvlModel.TryGetUnit(position).IsControlledByHuman() && !string.Equals(lvlModel.TryGetUnit(position).Name(), UnitType.Triangle))
                {
                    allEnemyPieces.Add(new Tuple<int, int>(x, y));
                }
            }
        }
        return allEnemyPieces;
    }

    public PieceMono SelectRandomPiece()
    {
        var allEnemyPieces = GetPieces();
        var allPieces = LevelMono.Instance._pieces;
        if (allEnemyPieces.Count < 2)
        {
            foreach (var enemy in allEnemyPieces)
            {
                if (!string.Equals(allPieces[enemy].Name(), UnitType.Triangle.ToString()) || allPieces[enemy].HasMoved() == true)
                {
                    allPieces[enemy].SetMoveState(false);
                    return null;
                }
                return allPieces[enemy];
            }
        }

        int attempts = 0;
        PieceMono result = null;
        while (result == null && attempts < allEnemyPieces.Count)
        {
            var randomPiece = allEnemyPieces[Random.Range(0, allEnemyPieces.Count)];

            if (!string.Equals(allPieces[randomPiece].Name(), UnitType.Triangle.ToString()))
            {
                if (allPieces[randomPiece].HasMoved() != true)
                {
                    result = allPieces[randomPiece];
                    LevelMono.Instance.storedCoord = randomPiece;
                    allPieces[randomPiece].SetMoveState(true);
                }
            }
            attempts++;
        }

        return result;
    }

    public void MovePiece()
    {
        StartCoroutine(DelayEnemyStart());
    }

    private void PerformTurn()
    {
        var piece = SelectRandomPiece();
        if (piece != null)
        {
            var moves = piece.GetLegalMoves(LevelMono.Instance.LevelModel.GetWidth(), LevelMono.Instance.LevelModel.GetHeight());
            int index = Random.Range(0, moves.Count);
            if (LevelMono.Instance.MovePiece(moves[index], piece))
            {
                GameManagerChain.Instance.MovedPieces.Add(piece);
                GameManagerChain.Instance.NumMoves += 1;
                MenuManager.Instance.ShowNumMovesInfo();
            }
        }

        if (GameManagerChain.Instance.NumMoves == 2 || piece == null)
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
