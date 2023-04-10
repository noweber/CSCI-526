using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Units;
using System;
using UnityEngine.SceneManagement;
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

    /*
     * Piece selection priority:
     *  1. move diamond to circles
     *  2. move a piece if it can capture (circle has capture priority)
     *  3. move circle if range enhanced by diamond
     *  4. select a random movable piece if all else fails
     */
    public Tuple<int, int> SelectBestPiece()
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

        //check for ANY piece that can capture (circle capture priority)
        var canCapture = GetPiecesThatCanCapture(movableEnemies);
        foreach (var piece in canCapture)
        {
            if (LevelMono.Instance.GetPiece(piece).IsCircle())
            {
                return piece;
            }
        }
        if (canCapture.Count > 0)
        {
            return canCapture[Random.Range(0, canCapture.Count)];
        }

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

    private List<Tuple<int, int>> GetPiecesThatCanCapture(List<Tuple<int, int>> movablePieces)
    {
        var levelMono = LevelMono.Instance;
        List<Tuple<int, int>> canCapture = new List<Tuple<int, int>>();
        foreach (var coord in movablePieces)
        {
            var piece = levelMono.GetPiece(coord);
            var moves = piece.LegalMoves(levelMono.GetWidth(), levelMono.GetHeight());
            bool added = false;
            foreach (var move in moves)
            {
                if (added)
                { 
                    break; 
                }
                if (IsACapturingMove(move))
                {
                    canCapture.Add(coord);
                    added = true;
                }
            }
        }

        return canCapture;
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
        var pieceAtDestination = LevelMono.Instance.GetPiece(destination);
        var lvlMono = LevelMono.Instance;
        var tile = lvlMono.GetTile(destination);

        //Capture if piece at destination not null and is Player and AI has visibility
        if (pieceAtDestination != null && pieceAtDestination.IsHuman() && tile.CanEnemySee())
        {
            return true;
        }
        return false;
    }

    private bool IsABaseCapture(Tuple<int, int> destination)
    {
        //if the destination move would be a capturing move, check if it is a base
        if (IsACapturingMove(destination))
        {
            return (LevelMono.Instance.GetPiece(destination).IsBase());
        }

        return false;
    }

	private bool IsAUnitCapture(Tuple<int, int> destination)
    {
        //if the destination move would be a capturing move, check if it is a base
        if (IsACapturingMove(destination))
        {
            return (LevelMono.Instance.GetPiece(destination).IsCircle() || LevelMono.Instance.GetPiece(destination).IsDiamond() || LevelMono.Instance.GetPiece(destination).IsScout());
        }

        return false;
    }

    private int PickBestMove(List<Tuple<int, int>> moves)
    {
        var lvlMono = LevelMono.Instance;
        int bestMove = -1;
        int minDistance = LevelMono.Instance.GetHeight() * LevelMono.Instance.GetWidth();

        List<int> capturingMoves = new List<int>();
        for (int i=0; i<moves.Count; i++)
        {
            //If AI has visibility, consider this move for capture
            if (lvlMono.GetTile(moves[i]).CanEnemySee())
            {
                if (IsACapturingMove(moves[i]))
                {
                    capturingMoves.Add(i);
                }
            }
        }
		// captures that can be done in current turn
        foreach (var index in capturingMoves)
        {
            bestMove = index;
            //prioritize base captures
            if (IsABaseCapture(moves[index]))
                break;
			if (IsAUnitCapture(moves[index]))
				break;
        }

		// find nearest human unit within vision
		if (bestMove == -1)
        {
            minDistance = LevelMono.Instance.GetHeight() * LevelMono.Instance.GetWidth();
            for (int i = 0; i < moves.Count; i++)
            {
                //move towards the base if you can see it
                foreach (var player in lvlMono.GetPlayerCoords())
                {
                    if (lvlMono.GetTile(player).CanEnemySee())
                    {
                        var distance = CalculateDistance(player, moves[i]);
                        if (distance < minDistance)
                        {
                            bestMove = i;
                            minDistance = distance;
                        }
                    }
                }
            }
        }

		// go towards satellites if nothing near to capture
		if (bestMove == -1)
        {
            minDistance = LevelMono.Instance.GetHeight() * LevelMono.Instance.GetWidth();
            for (int i = 0; i < moves.Count; i++)
            {
                //move towards the towers
                foreach (var tower in lvlMono.towerLocations)
                {
                    if (lvlMono.GetPiece(tower).IsHuman())
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

		// capture planet
        if (bestMove == -1)
        {
            minDistance = LevelMono.Instance.GetHeight() * LevelMono.Instance.GetWidth();
            for (int i = 0; i < moves.Count; i++)
            {
                //move towards the base if you can see it
                foreach (var playerBase in lvlMono.GetBaseCoords(true))
                {
                    if (lvlMono.GetTile(playerBase).CanEnemySee())
                    {
                        var distance = CalculateDistance(playerBase, moves[i]);
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
        var aiCoord = SelectBestPiece();
        /*if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            aiCoord = SelectRandomPiece();
        }*/

        if (aiCoord != null)
        {
            var aiPiece = lvlMono.GetPiece(aiCoord);
            lvlMono.SelectPiece(aiPiece, aiCoord);
            var moves = aiPiece.LegalMoves(lvlMono.GetWidth(), lvlMono.GetHeight());
            Tuple<int, int> destination = moves[Random.Range(0, moves.Count)];
            
            // Decide movement logic 
			/*
            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                // Random AI (with adjacent capture logic)
                // Subject to change
                Debug.Log("RANDOM AI");

                //If the player moves into enemy vision AND piece can capture: Capture.
                //Prioritize capturing bases over pieces
                foreach (var coord in moves)
                {
                    if (IsACapturingMove(coord))
                    {
                        destination = coord;
                        if (IsABaseCapture(coord))
                            break;
                    }
                }
            }
            else
            {
			*/
            Debug.Log("OPTIMAL AI");
            if (aiPiece.IsDiamond() && ShouldMoveDiamondToCircle(aiCoord))
            {
                destination = MoveDiamondToCircle(aiCoord, moves);
            }
            else
            {
                int index = PickBestMove(moves);
                destination = moves[index];
            }
            // }
            
            // Make AI movement based on above logic
            if (lvlMono.MovePiece(destination))
            {
                GameManagerChain.Instance.AddMovedPiece(aiPiece, destination);
                GameManagerChain.Instance.IncrementMoves(1);
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
        if (GameManagerChain.Instance.IsEnemyTurnOver() || aiCoord == null)
        {
            StopAllCoroutines();
            isRunning = false;
            StartCoroutine(GameManagerChain.Instance.StateToHuman());
            // GameManagerChain.Instance.ChangeState(GameStateEnum.Human);
        }
    }

    private IEnumerator DelayEnemyStart()
    {
        if (!isRunning)
        {
            isRunning = true;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(.4f);
        }
        MovePiece();
        yield return new WaitForSeconds(.4f);
        StartCoroutine(DelayEnemyStart());
    }
}
